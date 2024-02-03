using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class PlayerController : MonoBehaviour, IMovementEventCaster {
    public event Action<float> OnHorizontalInputRegistered;
    public event Action OnCasted;
    public event Action<bool> OnIsGroundedChanged;
    public event Action OnDashActivated;
    public event Action OnPotionConsumed;

    [SerializeField] private float m_MovementSpeed = 3f;
    [SerializeField] private float m_JumpStrength = 15f;
    [SerializeField] private float m_DashStrength = 15f;
    //GreenFireBall
    [SerializeField] private GameObject GFB;
    [SerializeField] private TextMeshProUGUI GFBCD;
    private int GFBTimer = 10;
    private float one = 1f;
    private bool GFBIsTiming = false;
    //BulletTime
    private bool bTime = false;
    private int bTimer = 10;
    private float two = 1f;
    [SerializeField] private TextMeshProUGUI BTCD;
    private int BTcooldown = 60;
    private float three = 1f;
    private bool BTCDon = false;

    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private float m_GroundRaycastDistance = 0.5f;

    private bool IsGrounded {
        get => m_IsGrounded;
        set {
            if (m_IsGrounded == value) return;

            this.OnIsGroundedChanged?.Invoke(value);
            m_IsGrounded = value;
        }
    }

    public ProjectileSpawner ProjectileSpawner {
        get { return m_ProjectileSpawner ??= GetComponent<ProjectileSpawner>(); }
    }

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private bool m_IsGrounded = false;

    private float m_HorizontalImpulse = 0;
    private float m_HorizontalImpulseFalloff = 7;
    private float m_HorizontalImpulseCutoff = 0.5f;

    private ContactPoint2D[] m_ContactPoints = new ContactPoint2D[32];

    private ProjectileSpawner m_ProjectileSpawner;

    private Rigidbody2D m_Rigidbody2D;
    private IMovementEventCaster m_MovementEventCasterImplementation;

    public void AddHorizontalImpulse(float impulse) {
        m_HorizontalImpulse += impulse;
    }

    private void Update() {
        if (Time.timeScale <= 0) return;

        if (Physics2D.Raycast(transform.position, Vector2.down, m_GroundRaycastDistance, m_GroundLayer.value)) {
            this.IsGrounded = true;
        } else {
            this.IsGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && this.IsGrounded) {
            float jumpStrength = m_JumpStrength;
            if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.LevitationBoots))
                jumpStrength *= 1.2f;
            this.Rigidbody2D.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        } else if (Input.GetButtonUp("Jump") && this.Rigidbody2D.velocity.y > 0) {
            this.Rigidbody2D.velocity = new Vector2(this.Rigidbody2D.velocity.x, this.Rigidbody2D.velocity.y / 2f);
        }

        if (Input.GetButtonDown("Fire1")) {
            if (PlayerState.Instance.Fireball.CanCast) {
                PlayerState.Instance.Fireball.Cast();
                this.ProjectileSpawner.LaunchWithARecoil(5, 15);
                this.OnCasted?.Invoke();
            }
        }

        if (Input.GetButtonDown("Fire2")) {
            if (PlayerState.Instance.UpgradesCollection.ContainsKey(PlayerState.PlayerUpgrade.HealingPotion)) {
                int potionCount = PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.HealingPotion];
                if (potionCount > 0) {
                    PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.HealingPotion]--;
                    this.OnPotionConsumed?.Invoke();
                }
            }
        }

        if (Input.GetButtonDown("Fire3")) {
            if (PlayerState.Instance.Dash.CanCast) {
                PlayerState.Instance.Dash.Cast();
                SFXPool.Instance.PlaySFX(SFXPool.SFX.Swoosh);
                AddHorizontalImpulse((-transform.right * transform.localScale.x * m_DashStrength).x);

                this.OnDashActivated?.Invoke();
            }
        }

        //New Spell Recover
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(PlayerState.Instance.Recover.CanCast)
            {
                PlayerState.Instance.Recover.Cast();
                this.OnPotionConsumed?.Invoke();
                PlayerState.Instance.Recovering();
            }
        }


        GFBCD.text = GFBTimer.ToString();
        BTCD.text = BTcooldown.ToString();

        //New Spell Green Fireball
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(PlayerState.Instance.GreenFireball.CanCast)
            {
                PlayerState.Instance.GreenFireball.Cast();
                this.ProjectileSpawner.Launch(GFB);
                this.OnCasted?.Invoke();
                GFBIsTiming = true;
            }
        }
        if(GFBIsTiming)
        {
            one -= Time.deltaTime;
            if (one <= 0)
            {
                GFBTimer -= 1;
                one = 1f;
            }
            if (GFBTimer <= 0)
            {
                GFBIsTiming = false;
                GFBTimer = 10;
            }
        }

        //New Spell Bullet Time
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(PlayerState.Instance.BulletTime.CanCast)
            {
                PlayerState.Instance.BulletTime.Cast();
                Time.timeScale = 0.5f;
                m_MovementSpeed = 6f;
                bTime = true;
                BTCDon = true;
            }
        }    
        if(bTime)
        {
            two -= Time.deltaTime;
            if (two <= 0)
            {
                bTimer -= 1;
                two = 1f;
            }
            if (bTimer <= 0)
            {
                bTime = false;
                bTimer = 10;
                Time.timeScale = 1f;
                m_MovementSpeed = 3f;
            }
        }
        if(BTCDon)
        {
            three -= Time.deltaTime;
            if (three <= 0)
            {
                BTcooldown -= 1;
                three = 1f;
            }
            if (BTcooldown <= 0)
            {
                bTime = false;
                BTcooldown = 60;
            }
        }
    }

    private void FixedUpdate() {
        ResolveCustomImpulseStep();

        float deltaX = Input.GetAxis("Horizontal");
        this.Rigidbody2D.velocity =
            new Vector2(deltaX * m_MovementSpeed + m_HorizontalImpulse, this.Rigidbody2D.velocity.y);
        this.OnHorizontalInputRegistered?.Invoke(deltaX);
    }

    private void ResolveCustomImpulseStep() {
        float oldImpulse = m_HorizontalImpulse;

        if (m_HorizontalImpulse != 0) {
            m_HorizontalImpulse -= Mathf.Sign(m_HorizontalImpulse) * m_HorizontalImpulseFalloff * Time.deltaTime;
        }

        if (Math.Sign(oldImpulse) != Math.Sign(m_HorizontalImpulse)
            || Mathf.Abs(m_HorizontalImpulse) < m_HorizontalImpulseCutoff) {
            m_HorizontalImpulse = 0;
            return;
        }

        int contacts = this.Rigidbody2D.GetContacts(m_ContactPoints);
        for (int i = 0; i < contacts; i++) {
            if (Mathf.Abs(m_ContactPoints[i].normal.y) < 0.01f &&
                Math.Sign(m_ContactPoints[i].normal.x) != Math.Sign(m_HorizontalImpulse) &&
                !Physics2D.GetIgnoreLayerCollision(10, 11)) {
                m_HorizontalImpulse = 0;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_GroundRaycastDistance);
    }
}
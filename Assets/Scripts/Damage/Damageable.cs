using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public event Action<float> OnCurrentHealthChanged;
    public event Action OnDamageTaken;
    public event Action OnDied;

    [SerializeField] private int m_HP = 100;
    [SerializeField] private float m_DamageIFrameTime = 0.5f;

    [SerializeField] private bool m_PushBack = false;

    [SerializeField] [ShowIf("m_PushBack")]
    private float m_PushBackStrength = 10;

    private bool Invulnerable = false;

    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponentInChildren<PlayerController>(); }
    }

    private List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    private bool CanTakeDamage =>
        this.CurrentHP > 0 && Time.time > m_DamageTakenTimeStamp + m_DamageIFrameTime && !Invulnerable;

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private int CurrentHP {
        get => m_CurrentHP;
        set {
            if (m_CurrentHP != value) {
                m_CurrentHP = value;
                this.OnCurrentHealthChanged?.Invoke(m_CurrentHP / (float) m_HP);
            }
        }
    }

    private Rigidbody2D m_Rigidbody2D;
    private PlayerController m_PlayerController;

    private int m_CurrentHP = Int32.MaxValue;
    private float m_DamageTakenTimeStamp = Single.MinValue;
    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();


    private void Awake() {
        this.CurrentHP = m_HP;
    }

    private void OnEnable() {
        if (this.PlayerController != null && PlayerState.Instance.HasDash2) {
            this.PlayerController.OnDashActivated += ActivateIFrames;
        }
    }

    private void OnDisable() {
        if (this.PlayerController != null && PlayerState.Instance.HasDash2) {
            this.PlayerController.OnDashActivated -= ActivateIFrames;
        }
    }

    public void TakeDamage(Transform damageOrigin, int damage) {
        if (!this.CanTakeDamage) return;

        if (m_PushBack) {
            if (this.PlayerController != null) {
                Vector3 impulse = (transform.position - damageOrigin.position).normalized * m_PushBackStrength;
                this.Rigidbody2D.AddForce(impulse.WithX(0),
                    ForceMode2D.Impulse);
                this.PlayerController.AddHorizontalImpulse(impulse.x);
            } else {
                this.Rigidbody2D.AddForce((transform.position - damageOrigin.position).normalized * m_PushBackStrength,
                    ForceMode2D.Impulse);
            }
        }

        m_DamageTakenTimeStamp = Time.time;
        this.CurrentHP -= damage;

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOColor(Color.red, m_DamageIFrameTime / 6f).SetLoops(6, LoopType.Yoyo);
        }

        this.OnDamageTaken?.Invoke();

        if (this.CurrentHP <= 0) {
            this.OnDied?.Invoke();
        }
    }

    private void ActivateIFrames() {
        StartCoroutine(ActivateIFramesCoroutine());
    }

    private IEnumerator ActivateIFramesCoroutine() {
        this.Invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOFade(0.5f, m_DamageIFrameTime / 6f).SetLoops(6, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(m_DamageIFrameTime);

        Physics2D.IgnoreLayerCollision(10, 11, false);
        this.Invulnerable = false;
    }
}
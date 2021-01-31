using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseAI : MonoBehaviour, IMovementEventCaster {
    public virtual event Action<float> OnHorizontalInputRegistered;
    public virtual event Action OnCasted;
    public virtual event Action<bool> OnIsGroundedChanged;

    [SerializeField] protected float m_Speed = 2f;
    [SerializeField] protected LayerMask m_GroundLayer;
    [SerializeField] protected Transform m_GroundRaycaster;
    [SerializeField] protected float m_GroundRaycastDistance;

    protected List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    protected PlayerController PlayerController {
        get { return m_PlayerController ??= this.SceneManager.PlayerController; }
    }

    protected SceneManager SceneManager {
        get { return m_SceneManager ??= GetComponentInParent<SceneManager>(); }
    }

    protected Damageable Damageable {
        get { return m_Damageable ??= GetComponentInChildren<Damageable>(); }
    }

    protected bool FreezeMovement = false;

    protected Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    protected float m_FreezeTime = 0.5f;

    private PlayerController m_PlayerController;

    private SceneManager m_SceneManager;

    private Damageable m_Damageable;

    private Rigidbody2D m_Rigidbody2D;


    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();

    protected void OnEnable() {
        this.Damageable.OnDamageTaken += InvokeTemporaryFreeze;
    }

    protected void OnDisable() {
        this.Damageable.OnDamageTaken -= InvokeTemporaryFreeze;
    }

    protected virtual void FixedUpdate() {
        if (!this.FreezeMovement) {
            MovementUpdate();
        }
    }

    protected virtual void MovementUpdate() {
    }

    protected virtual void InvokeTemporaryFreeze() {
        StartCoroutine(TemporaryFreezeCoroutine());
    }

    protected virtual IEnumerator TemporaryFreezeCoroutine() {
        this.Rigidbody2D.velocity = Vector2.zero;
        this.FreezeMovement = true;
        yield return new WaitForSeconds(m_FreezeTime);
        this.FreezeMovement = false;
    }
}
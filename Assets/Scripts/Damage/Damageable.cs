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
    [SerializeField] private float m_PerLevelIncrease = 0;
    [SerializeField] protected float m_DamageIFrameTime = 0.5f;

    [SerializeField] private bool m_PushBack = false;

    [SerializeField] [ShowIf("m_PushBack")]
    protected float m_PushBackStrength = 10;

    protected bool Invulnerable = false;

    protected List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    private bool CanTakeDamage =>
        this.CurrentHP > 0 && Time.time > m_DamageTakenTimeStamp + m_DamageIFrameTime && !Invulnerable;

    protected Rigidbody2D Rigidbody2D {
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

    private int m_CurrentHP = Int32.MaxValue;
    private float m_DamageTakenTimeStamp = Single.MinValue;
    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();


    private void Awake() {
        m_HP = (int) (m_HP + m_PerLevelIncrease * (PlayerState.Instance.Level - 1));
        this.CurrentHP = m_HP;
    }


    public void TakeDamage(Transform damageOrigin, int damage) {
        if (!this.CanTakeDamage) return;

        if (m_PushBack) {
            PushBack(damageOrigin);
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

    protected virtual void PushBack(Transform damageOrigin) {
        this.Rigidbody2D.AddForce((transform.position - damageOrigin.position).normalized * m_PushBackStrength,
            ForceMode2D.Impulse);
    }
}
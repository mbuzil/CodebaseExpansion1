using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public event Action OnDamageTaken;
    public event Action OnDied;

    [SerializeField] private int m_HP = 100;
    [SerializeField] private float m_DamageIFrameTime = 0.5f;

    private List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    private bool CanTakeDamage => m_CurrentHP > 0 && Time.time > m_DamageTakenTimeStamp + m_DamageIFrameTime;

    private int m_CurrentHP = Int32.MaxValue;
    private float m_DamageTakenTimeStamp = Single.MinValue;
    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();


    private void Awake() {
        m_CurrentHP = m_HP;
    }

    public void TakeDamage(int damage) {
        if (!this.CanTakeDamage) return;

        m_DamageTakenTimeStamp = Time.time;
        m_CurrentHP -= damage;

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOColor(Color.red, m_DamageIFrameTime / 6f).SetLoops(6, LoopType.Yoyo);
        }

        this.OnDamageTaken?.Invoke();

        if (m_CurrentHP <= 0) {
            this.OnDied?.Invoke();
        }
    }
}
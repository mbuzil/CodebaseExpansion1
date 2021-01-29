using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileCollision : SerializedMonoBehaviour {
    [SerializeField] private VFXPool.VFX? m_HitParticle = null;
    [SerializeField] private bool m_DestroyOnHit = true;
    [SerializeField] private int m_Damage = 25;

    [SerializeField] private bool m_FadeIn = true;

    private Collider Collider {
        get { return m_Collider ??= GetComponent<Collider>(); }
    }

    private List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    private Collider m_Collider;
    private GameObject m_Parent;
    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();

    private void OnDestroy() {
        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOKill();
        }
    }

    public void Init(GameObject parent, int damage = 25) {
        m_Parent = parent;
        m_Damage = damage;

        if (m_FadeIn) {
            foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
                spriteRenderer.color = spriteRenderer.color.WithAlpha(0);
                spriteRenderer.DOFade(1, 0.25f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == m_Parent) {
            return;
        }

        Damageable damageable = other.GetComponentInChildren<Damageable>();
        if (damageable != null) {
            damageable.TakeDamage(m_Damage);
        }

        if (m_DestroyOnHit) {
            if (m_HitParticle != null) {
                VFXPool.Instance?.PlayVFX(m_HitParticle.Value, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
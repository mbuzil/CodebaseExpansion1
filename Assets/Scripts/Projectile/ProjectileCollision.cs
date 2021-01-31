using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileCollision : SerializedMonoBehaviour {
    [SerializeField] private VFXPool.VFX? m_HitParticle = null;
    [SerializeField] private bool m_DestroyOnHit = true;
    [SerializeField] private int m_Damage = 25;
    [SerializeField] private float m_PerLevelIncrease = 0;

    [SerializeField] private bool m_FadeIn = true;
    [SerializeField] private GameObject m_Parent;

    private List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    public Collider2D Collider2D {
        get { return m_Collider2D ??= GetComponent<Collider2D>(); }
    }

    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();

    private Collider2D m_Collider2D;

    private void Awake() {
        m_Damage = (int) (m_Damage + m_PerLevelIncrease * (PlayerState.Instance.Level - 1));
    }

    private void OnDestroy() {
        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOKill();
        }
    }

    public void Init(GameObject parent) {
        m_Parent = parent;

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
            damageable.TakeDamage(transform, m_Damage);
            StartCoroutine(ToggleCollidersOnOff());
        }

        if (m_DestroyOnHit) {
            if (m_HitParticle != null) {
                VFXPool.Instance?.PlayVFX(m_HitParticle.Value, transform.position);
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator ToggleCollidersOnOff() {
        this.Collider2D.enabled = false;
        yield return new WaitForSeconds(1f);
        this.Collider2D.enabled = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyDeathRoutine : BaseDeathRoutine {
    private ProjectileCollision ProjectileCollision {
        get { return m_ProjectileCollision ??= GetComponentInChildren<ProjectileCollision>(); }
    }

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private List<DOTweenAnimation> DOTweenAnimations {
        get {
            if (m_DOTweenAnimation.Count == 0) {
                m_DOTweenAnimation = GetComponentsInChildren<DOTweenAnimation>().ToList();
            }

            return m_DOTweenAnimation;
        }
    }

    private List<SpriteRenderer> SpriteRenderers {
        get {
            if (m_SpriteRenderers.Count == 0) {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            }

            return m_SpriteRenderers;
        }
    }

    private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();

    private ProjectileCollision m_ProjectileCollision;

    private Rigidbody2D m_Rigidbody2D;

    private List<DOTweenAnimation> m_DOTweenAnimation = new List<DOTweenAnimation>();

    protected override void Die() {
        base.Die();
        this.ProjectileCollision.enabled = false;
        this.Rigidbody2D.isKinematic = false;
        this.Rigidbody2D.gravityScale = 3f;
        foreach (DOTweenAnimation animation in this.DOTweenAnimations) {
            animation.DOKill();
        }

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOFade(0, 1f).OnComplete(() => Destroy(gameObject));
        }
    }
}
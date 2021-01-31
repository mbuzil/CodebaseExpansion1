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
        get { return m_DOTweenAnimation ??= GetComponentsInChildren<DOTweenAnimation>().ToList(); }
    }

    private List<SpriteRenderer> SpriteRenderers {
        get { return m_SpriteRenderers ??= GetComponentsInChildren<SpriteRenderer>().ToList(); }
    }

    private List<SpriteRenderer> m_SpriteRenderers = null;

    private ProjectileCollision m_ProjectileCollision = null;

    private Rigidbody2D m_Rigidbody2D;

    private List<DOTweenAnimation> m_DOTweenAnimation = null;

    protected override void Die() {
        base.Die();
        
        this.ProjectileCollision.enabled = false;
        this.Rigidbody2D.isKinematic = false;
        this.Rigidbody2D.gravityScale = 3f;
        
        foreach (DOTweenAnimation animation in this.DOTweenAnimations) {
            animation.DOKill();
        }

        for (int i = 0; i < this.SpriteRenderers.Count; i++) {
            if (i == 0) {
                this.SpriteRenderers[i].DOFade(0, 1f).OnComplete(() => Destroy(gameObject));
            } else {
                this.SpriteRenderers[i].DOFade(0, 1f);
            }
        }
    }
}
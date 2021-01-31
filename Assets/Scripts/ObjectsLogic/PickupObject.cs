using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PickupObject : BaseProgrammaticAnimator {
    [SerializeField] private int m_CoinValue = 0;

    private bool m_Active = true;

    private List<SpriteRenderer> SpriteRenderers {
        get { return m_SpriteRenderers ??= GetComponentsInChildren<SpriteRenderer>().ToList(); }
    }

    private List<SpriteRenderer> m_SpriteRenderers = null;

    private static readonly int Disappear = Animator.StringToHash("disappear");

    private void OnTriggerEnter2D(Collider2D other) {
        if (!m_Active) return;

        PlayerController playerController = other.GetComponentInChildren<PlayerController>();
        if (playerController != null) {
            m_Active = false;

            AnimateDisappearance();
        }
    }

    private void AnimateDisappearance() {
        if (ParameterExists(Disappear)) {
            this.Animator.SetTrigger(Disappear);
        }

        transform.DOPop();

        PlayerState.Instance.AddCoins(m_CoinValue);

        for (int i = 0; i < this.SpriteRenderers.Count; i++) {
            if (i == 0) {
                this.SpriteRenderers[i].DOFade(0, 1f).OnComplete(() => Destroy(gameObject));
            } else {
                this.SpriteRenderers[i].DOFade(0, 1f);
            }
        }
    }
}
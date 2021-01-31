using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerDamagable : Damageable {
    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponentInChildren<PlayerController>(); }
    }


    private PlayerController m_PlayerController;

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

    protected override void PushBack(Transform damageOrigin) {
        Vector3 impulse = (transform.position - damageOrigin.position).normalized * m_PushBackStrength;
        this.Rigidbody2D.AddForce(impulse.WithX(0),
            ForceMode2D.Impulse);
        this.PlayerController.AddHorizontalImpulse(impulse.x);
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
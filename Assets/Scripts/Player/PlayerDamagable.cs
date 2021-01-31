using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerDamagable : Damageable {
    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponentInChildren<PlayerController>(); }
    }

    private PlayerController m_PlayerController;

    protected override void Awake() {
        int increases = 0;
        if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.EnchantedMaterials)) {
            increases = PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.EnchantedMaterials];
        }

        m_HP = (int) (m_HP + 15 * increases);
        this.CurrentHP = m_HP;
    }

    private void OnEnable() {
        if (this.PlayerController != null &&
            PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.DimensionalShift)) {
            this.PlayerController.OnDashActivated += ActivateIFrames;
        }

        this.PlayerController.OnPotionConsumed += ActivationHealthPotion;
    }

    private void OnDisable() {
        if (this.PlayerController != null && PlayerState.Instance != null &&
            PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.DimensionalShift)) {
            this.PlayerController.OnDashActivated -= ActivateIFrames;
        }

        this.PlayerController.OnPotionConsumed -= ActivationHealthPotion;
    }

    protected override void PushBack(Transform damageOrigin) {
        Vector3 impulse = (transform.position - damageOrigin.position).normalized * m_PushBackStrength;
        this.Rigidbody2D.AddForce(impulse.WithX(0),
            ForceMode2D.Impulse);
        this.PlayerController.AddHorizontalImpulse(impulse.x);
    }

    private void ActivationHealthPotion() {
        this.CurrentHP = m_HP;
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
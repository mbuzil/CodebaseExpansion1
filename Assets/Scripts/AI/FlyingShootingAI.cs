using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShootingAI : BaseFlyingAI {
    public override event Action OnCasted;

    [SerializeField] private float m_ShootingCooldown = 3f;

    private ProjectileSpawner ProjectileSpawner {
        get { return m_ProjectileSpawner ??= GetComponentInChildren<ProjectileSpawner>(); }
    }

    private ProjectileSpawner m_ProjectileSpawner;

    private float m_NextShotCooldown = float.MinValue;

    private void Awake() {
        m_FreezeTime = 1f;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (!this.FreezeMovement) {
            if (CheckClearTowardsPlayer() && Time.time > m_NextShotCooldown) {
                float angleTowards = Vector2.SignedAngle(
                    transform.localScale.x < 0 ? -transform.right : transform.right,
                    this.PlayerController.transform.position - m_GroundRaycaster.position
                ) * (transform.localScale.x < 0 ? -1 : 1);
                m_NextShotCooldown = Time.time + m_ShootingCooldown;

                this.ProjectileSpawner.LaunchWithARecoil(-3f + angleTowards, 3f + angleTowards);
                this.OnCasted?.Invoke();
            }
        }
    }
}
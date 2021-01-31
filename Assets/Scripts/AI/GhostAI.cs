using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostAI : FlyingShootingAI {
    [SerializeField] private float m_AggroRange = 10f;
    [SerializeField] private float m_TeleportCD = 2f;
    [SerializeField] private Vector2 m_TeleportRange = new Vector2(3, 8);

    private float m_NextTeleportTimestamp = float.MinValue;

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (!this.FreezeMovement) {
            if (Time.time > m_NextTeleportTimestamp &&
                (this.PlayerController.transform.position - m_GroundRaycaster.position).magnitude < m_AggroRange) {
                m_NextTeleportTimestamp = Time.time + m_TeleportCD;
                StartCoroutine(TeleportCoroutine());
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_AggroRange);
    }

    private IEnumerator TeleportCoroutine() {
        this.FreezeMovement = true;

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOFade(0f, 0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 randomDirection = Vector3.zero;
        while (randomDirection.x == 0 && randomDirection.y == 0) {
            randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        transform.position = this.PlayerController.transform.position +
                             randomDirection * Random.Range(m_TeleportRange.x, m_TeleportRange.y);

        foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
            spriteRenderer.DOFade(1f, 0.5f);
        }

        this.FreezeMovement = false;
    }
}
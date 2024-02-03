using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileSpawner : MonoBehaviour {
    [SerializeField] private float m_LaunchDelay = 0.1f;

    [SerializeField] private GameObject m_ProjectilePrefab;

    [SerializeField] private float m_ElevationAngle = 0;
    [SerializeField] private float m_ProjectileSpeed = 0;

    [SerializeField] private Transform m_ProjectileOrigin;

    public void SetElevationAngle(float angle) {
        m_ElevationAngle = angle;
    }

    public void LaunchWithARecoil(float minAngle, float maxAngle) {
        SetElevationAngle(Random.Range(minAngle, maxAngle));
        Launch(m_ProjectilePrefab);
    }

    public void Launch(GameObject proj) {//changed to make it take a GameObject
        StartCoroutine(DelayedLaunchCoroutine(proj));
    }

    private IEnumerator DelayedLaunchCoroutine(GameObject proj) {//Changed to make it take a GameObject
        yield return new WaitForSeconds(m_LaunchDelay);

        SFXPool.Instance.PlaySFX(SFXPool.SFX.SpellCast1);

        GameObject projectile = Instantiate(proj);
        projectile.transform.position = m_ProjectileOrigin.position;

        ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();
        movement.Init(
            transform.right * -transform.localScale.x
            + new Vector3(0, Mathf.Sin(m_ElevationAngle * Mathf.Deg2Rad)),
            m_ProjectileSpeed);

        ProjectileCollision collision = projectile.GetComponent<ProjectileCollision>();
        collision.Init(gameObject);
    }
}
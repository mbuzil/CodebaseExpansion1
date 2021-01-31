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
        Launch();
    }

    public void Launch() {
        StartCoroutine(DelayedLaunchCoroutine());
    }

    private IEnumerator DelayedLaunchCoroutine() {
        yield return new WaitForSeconds(m_LaunchDelay);

        GameObject projectile = Instantiate(m_ProjectilePrefab);
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
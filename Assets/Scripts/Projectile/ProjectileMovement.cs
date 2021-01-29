using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {
    [SerializeField] private Vector3 m_MovementDirection;
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_FalloffSpeed;

    private bool m_Initialized = false;

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private Rigidbody2D m_Rigidbody2D;

    [Button]
    public void Init(Vector3 direction, float speed) {
        m_MovementDirection = direction;
        m_MovementSpeed = speed;

        this.Rigidbody2D.velocity = m_MovementDirection.normalized * m_MovementSpeed;
        m_Initialized = true;
    }

    private void FixedUpdate() {
        if (m_Initialized) {
            this.Rigidbody2D.velocity += Vector2.down * m_FalloffSpeed;
            UpdateSpriteRotation();
        }
    }

    private void UpdateSpriteRotation() {
        float angle = Vector2.SignedAngle(this.Rigidbody2D.velocity, Vector2.right);
        transform.rotation = Quaternion.Euler(0, 0, 180 - angle);
    }
}
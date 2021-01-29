using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    public event Action<float> OnPlayerHorizontalInputRegistered;
    public event Action OnPlayerCasted;
    public event Action<bool> OnIsGroundedChanged;

    [SerializeField] private float m_MovementSpeed = 3f;
    [SerializeField] private float m_JumpStrength = 15f;

    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private float m_GroundRaycastDistance = 0.5f;

    private bool IsGrounded {
        get => m_IsGrounded;
        set {
            if (m_IsGrounded == value) return;

            this.OnIsGroundedChanged?.Invoke(value);
            m_IsGrounded = value;
        }
    }

    private ProjectileSpawner ProjectileSpawner {
        get { return m_ProjectileSpawner ??= GetComponent<ProjectileSpawner>(); }
    }

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private bool m_IsGrounded = false;

    private ProjectileSpawner m_ProjectileSpawner;

    private Rigidbody2D m_Rigidbody2D;

    private void Update() {
        if (Physics2D.Raycast(transform.position, Vector2.down, m_GroundRaycastDistance, m_GroundLayer.value)) {
            this.IsGrounded = true;
        } else {
            this.IsGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && this.IsGrounded) {
            this.Rigidbody2D.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        } else if (Input.GetButtonUp("Jump") && this.Rigidbody2D.velocity.y > 0) {
            this.Rigidbody2D.velocity = new Vector2(this.Rigidbody2D.velocity.x, this.Rigidbody2D.velocity.y / 2f);
        }

        if (Input.GetButtonDown("Fire1")) {
            this.ProjectileSpawner.LaunchWithARecoil(5, 15);
            this.OnPlayerCasted?.Invoke();
        }
    }

    private void FixedUpdate() {
        float deltaX = Input.GetAxis("Horizontal");
        this.Rigidbody2D.velocity = new Vector2(deltaX * m_MovementSpeed, this.Rigidbody2D.velocity.y);
        this.OnPlayerHorizontalInputRegistered?.Invoke(deltaX);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_GroundRaycastDistance);
    }
}
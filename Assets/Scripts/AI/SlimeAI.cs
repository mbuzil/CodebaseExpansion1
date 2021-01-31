using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeAI : MonoBehaviour, IMovementEventCaster {
    public event Action<float> OnHorizontalInputRegistered;
    public event Action OnCasted;
    public event Action<bool> OnIsGroundedChanged;

    [SerializeField] private float m_MovementSpeed = 2f;
    [SerializeField] private Transform m_GroundRaycaster;
    [SerializeField] private float m_GroundRaycastDistance;
    [SerializeField] private Transform m_ForwardRaycaster;
    [SerializeField] private float m_ForwardRaycastDistance;

    [SerializeField] private LayerMask m_GroundLayer;

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private Rigidbody2D m_Rigidbody2D;

    private void Update() {
        Vector2 movementDirection = Vector2.right;
        if (transform.localScale.x > 0) movementDirection = -Vector2.right;

        if (Physics2D.Raycast(m_GroundRaycaster.position, Vector2.down, m_GroundRaycastDistance, m_GroundLayer.value)
            && !Physics2D.Raycast(m_ForwardRaycaster.position, movementDirection, m_ForwardRaycastDistance,
                m_GroundLayer.value)) {
            this.Rigidbody2D.velocity =
                new Vector2((movementDirection * m_MovementSpeed).x, this.Rigidbody2D.velocity.y);
            this.OnHorizontalInputRegistered?.Invoke(movementDirection.x);
        } else {
            this.Rigidbody2D.velocity = new Vector2(0, this.Rigidbody2D.velocity.y);
            this.OnHorizontalInputRegistered?.Invoke(-movementDirection.x);
        }
    }
}
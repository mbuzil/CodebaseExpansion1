using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeAI : BaseAI {
    public override event Action<float> OnHorizontalInputRegistered;
    public override event Action OnCasted;
    public override event Action<bool> OnIsGroundedChanged;
    
    [SerializeField] protected Transform m_ForwardRaycaster;
    [SerializeField] protected float m_ForwardRaycastDistance;

    protected override void MovementUpdate() {
        Vector2 movementDirection = Vector2.right;
        if (transform.localScale.x > 0) movementDirection = -Vector2.right;

        if (Physics2D.Raycast(m_GroundRaycaster.position, Vector2.down, m_GroundRaycastDistance, m_GroundLayer.value)
            && !Physics2D.Raycast(m_ForwardRaycaster.position, movementDirection, m_ForwardRaycastDistance,
                m_GroundLayer.value)) {
            this.Rigidbody2D.velocity =
                new Vector2((movementDirection * m_Speed).x, this.Rigidbody2D.velocity.y);
            this.OnHorizontalInputRegistered?.Invoke(movementDirection.x);
        } else {
            this.Rigidbody2D.velocity = new Vector2(0, this.Rigidbody2D.velocity.y);
            this.OnHorizontalInputRegistered?.Invoke(-movementDirection.x);
            this.InvokeTemporaryFreeze();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseFlyingAI : BaseAI {
    public override event Action<float> OnHorizontalInputRegistered;
    public override event Action OnCasted;
    public override event Action<bool> OnIsGroundedChanged;

    [SerializeField] private float m_MaxTimeInOneDirection = 5f;
    [SerializeField] private float m_MinTimeInOneDirection = 1f;

    private Vector2 m_FlightDirection = new Vector2(0, 0);
    private float m_NextDirectionChangeTimestamp = float.MinValue;

    protected override void MovementUpdate() {
        if (CheckClearTowardsPlayer()) {
            m_FlightDirection = (this.PlayerController.transform.position - m_GroundRaycaster.position).normalized;
        }

        if (Time.time > m_NextDirectionChangeTimestamp || m_FlightDirection == Vector2.zero
                                                       || Physics2D.Raycast(m_GroundRaycaster.position,
                                                           m_FlightDirection, m_GroundRaycastDistance,
                                                           m_GroundLayer.value)) {
            ChangeFlightDirection();
            this.Rigidbody2D.velocity = Vector2.zero;
            return;
        }

        this.Rigidbody2D.velocity = m_FlightDirection * m_Speed;
        this.OnHorizontalInputRegistered?.Invoke(this.Rigidbody2D.velocity.x);
    }

    protected bool CheckClearTowardsPlayer() {
        return !Physics2D.Linecast(m_GroundRaycaster.position, this.PlayerController.transform.position,
            m_GroundLayer);
    }


    private void ChangeFlightDirection() {
        m_NextDirectionChangeTimestamp = Time.time + Random.Range(m_MinTimeInOneDirection, m_MaxTimeInOneDirection);

        m_FlightDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
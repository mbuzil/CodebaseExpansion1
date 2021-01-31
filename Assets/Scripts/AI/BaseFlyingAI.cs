using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseFlyingAI : MonoBehaviour, IMovementEventCaster {
    public event Action<float> OnHorizontalInputRegistered;
    public event Action OnCasted;
    public event Action<bool> OnIsGroundedChanged;

    [SerializeField] private float m_Speed = 3f;
    [SerializeField] private float m_MaxTimeInOneDirection = 5f;
    [SerializeField] private float m_MinTimeInOneDirection = 1f;

    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Transform m_GroundRaycaster;
    [SerializeField] private float m_GroundRaycastDistance = 0.5f;

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private Rigidbody2D m_Rigidbody2D;

    private Vector2 m_FlightDirection = new Vector2(0, 0);
    private float m_NextDirectionChangeTimestamp = float.MinValue;

    private void FixedUpdate() {
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

    private void CheckClearTowardsPlayer() {
        
    }


    private void ChangeFlightDirection() {
        m_NextDirectionChangeTimestamp = Time.time + Random.Range(m_MinTimeInOneDirection, m_MaxTimeInOneDirection);

        m_FlightDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
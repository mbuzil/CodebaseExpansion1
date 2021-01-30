using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour {
    [SerializeField] private Animator m_CastingHandAnimator;

    private Damageable Damageable {
        get { return m_Damageable ??= GetComponent<Damageable>(); }
    }

    private Animator Animator {
        get { return m_Animator ??= GetComponentInChildren<Animator>(); }
    }

    private IMovementEventCaster MovementEventCaster {
        get { return m_MovementEventCaster ??= GetComponent<IMovementEventCaster>(); }
    }

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private HashSet<int> m_ParameterExistenceSet = new HashSet<int>();

    private Damageable m_Damageable;

    private Animator m_Animator;

    private IMovementEventCaster m_MovementEventCaster;

    private Rigidbody2D m_Rigidbody2D;

    private static readonly int HorizontalInputAbs = Animator.StringToHash("horizontalInputAbs");
    private static readonly int VerticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int VerticalVelocityAbs = Animator.StringToHash("verticalVelocityAbs");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int Cast = Animator.StringToHash("cast");
    private static readonly int Die = Animator.StringToHash("die");

    private void Awake() {
        for (int i = 0; i < this.Animator.parameterCount; i++) {
            m_ParameterExistenceSet.Add(this.Animator.parameters[i].nameHash);
        }
    }

    private void OnEnable() {
        this.MovementEventCaster.OnHorizontalInputRegistered += UpdateSpriteRendererFlip;
        this.MovementEventCaster.OnCasted += TriggerCastedAnimation;
        this.MovementEventCaster.OnIsGroundedChanged += UpdateIsGroundedState;
        this.Damageable.OnDied += TriggerDeathAnimation;
    }

    private void OnDisable() {
        this.MovementEventCaster.OnHorizontalInputRegistered -= UpdateSpriteRendererFlip;
        this.MovementEventCaster.OnCasted -= TriggerCastedAnimation;
        this.MovementEventCaster.OnIsGroundedChanged -= UpdateIsGroundedState;
        this.Damageable.OnDied -= TriggerDeathAnimation;
    }

    private void Update() {
        if (ParameterExists(VerticalVelocity))
            this.Animator.SetFloat(VerticalVelocity, this.Rigidbody2D.velocity.y);
        if (ParameterExists(VerticalVelocityAbs))
            this.Animator.SetFloat(VerticalVelocityAbs, Mathf.Abs(this.Rigidbody2D.velocity.y));
    }

    private bool ParameterExists(int nameHash) {
        return m_ParameterExistenceSet.Contains(nameHash);
    }

    private void UpdateIsGroundedState(bool isGrounded) {
        if (ParameterExists(IsGrounded))
            this.Animator.SetBool(IsGrounded, isGrounded);
    }

    private void UpdateSpriteRendererFlip(float horizontalInput) {
        if (horizontalInput > 0) {
            this.transform.localScale = new Vector2(-1, 1);
        } else if (horizontalInput < 0) {
            this.transform.localScale = new Vector2(1, 1);
        }

        if (ParameterExists(HorizontalInputAbs))
            this.Animator.SetFloat(HorizontalInputAbs, Mathf.Abs(horizontalInput));
    }

    private void TriggerCastedAnimation() {
        if (m_CastingHandAnimator) {
            this.m_CastingHandAnimator.SetTrigger(Cast);
        } else {
            if (ParameterExists(Cast))
                this.Animator.SetTrigger(Cast);
        }
    }

    private void TriggerDeathAnimation() {
        if (ParameterExists(Die))
            this.Animator.SetTrigger(Die);
    }
}
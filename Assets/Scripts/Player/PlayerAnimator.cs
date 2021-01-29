using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private Animator m_CastingHandAnimator;

    private Animator Animator {
        get { return m_Animator ??= GetComponent<Animator>(); }
    }

    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponent<PlayerController>(); }
    }

    private Rigidbody2D Rigidbody2D {
        get { return m_Rigidbody2D ??= GetComponent<Rigidbody2D>(); }
    }

    private SpriteRenderer SpriteRenderer {
        get { return m_SpriteRenderer ??= GetComponent<SpriteRenderer>(); }
    }

    private Animator m_Animator;

    private PlayerController m_PlayerController;

    private Rigidbody2D m_Rigidbody2D;

    private SpriteRenderer m_SpriteRenderer;

    private static readonly int HorizontalInputAbs = Animator.StringToHash("horizontalInputAbs");
    private static readonly int VerticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int VerticalVelocityAbs = Animator.StringToHash("verticalVelocityAbs");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int Cast = Animator.StringToHash("cast");

    private void OnEnable() {
        this.PlayerController.OnPlayerHorizontalInputRegistered += UpdateSpriteRendererFlip;
        this.PlayerController.OnPlayerCasted += TriggerPlayerCastedAnimation;
        this.PlayerController.OnIsGroundedChanged += UpdateIsGroundedState;
    }

    private void OnDisable() {
        this.PlayerController.OnPlayerHorizontalInputRegistered -= UpdateSpriteRendererFlip;
        this.PlayerController.OnPlayerCasted -= TriggerPlayerCastedAnimation;
        this.PlayerController.OnIsGroundedChanged -= UpdateIsGroundedState;
    }

    private void UpdateIsGroundedState(bool isGrounded) {
        this.Animator.SetBool(IsGrounded, isGrounded);
    }

    private void UpdateSpriteRendererFlip(float horizontalInput) {
        if (horizontalInput > 0) {
            this.transform.localScale = new Vector2(-1, 1);
        } else if (horizontalInput < 0) {
            this.transform.localScale = new Vector2(1, 1);
        }

        this.Animator.SetFloat(HorizontalInputAbs, Mathf.Abs(horizontalInput));
    }

    private void TriggerPlayerCastedAnimation() {
        this.m_CastingHandAnimator?.SetTrigger(Cast);
    }

    private void Update() {
        this.Animator.SetFloat(VerticalVelocity, this.Rigidbody2D.velocity.y);
        this.Animator.SetFloat(VerticalVelocityAbs, Mathf.Abs(this.Rigidbody2D.velocity.y));
    }
}
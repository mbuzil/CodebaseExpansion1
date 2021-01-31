using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerDeathRoutine : BaseDeathRoutine {
    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponentInChildren<PlayerController>(); }
    }

    private PlayerController m_PlayerController;

    protected override void Die() {
        base.Die();
        this.PlayerController.enabled = false;
    }
}
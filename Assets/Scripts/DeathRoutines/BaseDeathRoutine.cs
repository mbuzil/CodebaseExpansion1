using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseDeathRoutine : MonoBehaviour {
    protected Damageable Damageable {
        get { return m_Damageable ??= GetComponentInChildren<Damageable>(); }
    }

    private IMovementEventCaster MovementEventCaster {
        get { return m_MovementEventCaster ??= GetComponentInChildren<IMovementEventCaster>(); }
    }

    private EntityAnimator EntityAnimator {
        get { return m_EntityAnimator ??= GetComponentInChildren<EntityAnimator>(); }
    }

    private Damageable m_Damageable;

    private IMovementEventCaster m_MovementEventCaster;

    private EntityAnimator m_EntityAnimator;

    private void OnEnable() {
        this.Damageable.OnDied += InvokeDelayedDeathResponse;
    }

    private void OnDisable() {
        this.Damageable.OnDied += InvokeDelayedDeathResponse;
    }


    [Button]
    private void InvokeDelayedDeathResponse() {
        StartCoroutine(DelayedDeathResponse());
    }

    private IEnumerator DelayedDeathResponse() {
        yield return new WaitForEndOfFrame();
        Die();
    }

    protected virtual void Die() {
        ((MonoBehaviour) this.MovementEventCaster).enabled = false;
        this.EntityAnimator.enabled = false;
        this.Damageable.enabled = false;
    }
}
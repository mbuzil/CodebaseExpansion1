using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationTester : MonoBehaviour, IMovementEventCaster {
    public event Action<float> OnHorizontalInputRegistered;
    public event Action OnCasted;
    public event Action<bool> OnIsGroundedChanged;

    private void Awake() {
        StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine() {
        yield return null;

        while (true) {
            this.OnCasted?.Invoke();
            yield return new WaitForSeconds(2f);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExfilLocation : TooltipInteractibleObject {
    public event Action OnExfiled;

    public void Update() {
        if (m_TooltipActive && Input.GetKeyDown(KeyCode.F)) {
            this.OnExfiled?.Invoke();
            enabled = false;
        }
    }
}
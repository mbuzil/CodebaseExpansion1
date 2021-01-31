using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoreNote : TooltipInteractibleObject {
    public event Action OnNoteActivated;

    public void Update() {
        if (m_TooltipActive && Input.GetKeyDown(KeyCode.F) && Time.timeScale > 0) {
            this.OnNoteActivated?.Invoke();
        }
    }
}
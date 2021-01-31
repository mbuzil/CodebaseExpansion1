using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability {
    private float m_LastCast = float.MinValue;

    public float Cooldown { get; set; } = 3f;
    public bool Locked { get; set; } = false;
    public Image AbilityImage { get; set; } = null;


    public bool CanCast => Time.time > m_LastCast + this.Cooldown && !this.Locked;

    public void UpdateAssociatedGraphics() {
        if (this.AbilityImage == null) return;
        this.AbilityImage.fillAmount = (Time.time - m_LastCast) / this.Cooldown;
    }

    public void Cast() {
        if (!this.CanCast || this.Locked) return;
        m_LastCast = Time.time;
    }
}
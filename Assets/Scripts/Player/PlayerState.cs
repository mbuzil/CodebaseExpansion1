using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    public event Action<int> OnCoinsAmountChanged;
    public event Action<int> OnCoinsAdded;

    public static PlayerState Instance = null;

    public int Level = 1;
    public bool HasDash = false;
    public bool HasDash2 = false;

    public readonly Ability Fireball = new Ability();
    public readonly Ability Dash = new Ability();

    public int Coins {
        get => m_Coins;
        private set {
            if (m_Coins != value) {
                m_Coins = value;
                this.OnCoinsAmountChanged?.Invoke(m_Coins);
            }
        }
    }

    private int m_Coins = 0;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetPlayerState();
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void Update() {
        this.Fireball.UpdateAssociatedGraphics();
        this.Dash.UpdateAssociatedGraphics();
    }

    public void ResetPlayerState() {
        this.Fireball.Cooldown = 2.25f;
        this.Dash.Cooldown = 1;

        this.Dash.Locked = !this.HasDash;

        this.Coins = 0;
        this.Level = 1;
    }

    public void AddCoins(int amountToAdd) {
        if (amountToAdd == 0) return;

        this.Coins += amountToAdd;
        this.OnCoinsAdded?.Invoke(amountToAdd);
    }
}
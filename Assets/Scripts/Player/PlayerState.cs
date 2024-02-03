using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

public class PlayerState : SerializedMonoBehaviour {

    //for cooldown onscreen
    [SerializeField] TextMeshProUGUI RecoverCD;
    private int RCD = 25;
    private float one = 1f;
    private bool recoverying = false;

    


    public enum PlayerUpgrade {
        LevitationBoots,
        SignetOfReplication,
        SpellslignersGloves,
        TemporalWarp,
        DimensionalShift,
        AnticorrosiveLining,
        GarlicVileNecklace,
        VoidLens,
        SoulMirror,
        TomeOfWisdom,
        EnchantedMaterials,
        HealingPotion
    }

    public event Action<int> OnCoinsAmountChanged;
    public event Action<int> OnCoinsAdded;

    public static PlayerState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = FindObjectOfType<PlayerState>();

                if (m_Instance != null) {
                    DontDestroyOnLoad(m_Instance.gameObject);
                    m_Instance.ResetPlayerState();
                }
            }

            return m_Instance;
        }
    }

    public int Level = 1;

    public Dictionary<PlayerUpgrade, int> UpgradesCollection = new Dictionary<PlayerUpgrade, int>();

    [NonSerialized] public readonly Ability Fireball = new Ability();
    [NonSerialized] public readonly Ability Dash = new Ability();
    //New Spells
    [NonSerialized] public readonly Ability Recover = new Ability();
    [NonSerialized] public readonly Ability GreenFireball = new Ability();
    [NonSerialized] public readonly Ability BulletTime = new Ability();

    private static PlayerState m_Instance = null;

    public bool HasUpgrade(PlayerUpgrade playerUpgrade) {
        return this.UpgradesCollection.ContainsKey(playerUpgrade);
    }

    public void AddUpgrade(PlayerUpgrade playerUpgrade) {
        if (playerUpgrade == PlayerUpgrade.SpellslignersGloves) {
            this.Fireball.Cooldown /= 2f;
        }

        if (this.UpgradesCollection.ContainsKey(playerUpgrade)) {
            this.UpgradesCollection[playerUpgrade]++;
        } else {
            this.UpgradesCollection.Add(playerUpgrade, 1);
        }
    }

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
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetPlayerState();
        } else {
            if (Instance == this) return;
            Destroy(gameObject);
            return;
        }
    }

    public void Update() {
        this.Dash.Locked = !HasUpgrade(PlayerUpgrade.TemporalWarp);

        this.Fireball.UpdateAssociatedGraphics();
        this.GreenFireball.UpdateAssociatedGraphics();
        this.Dash.UpdateAssociatedGraphics();

        RecoverCD.text = RCD.ToString();//onscreen cooldown
        if (recoverying)
        {
            one -= Time.deltaTime;
            if (one <= 0) 
            { 
                RCD -= 1;
                one = 1f;
            }
            if (RCD <= 0)
            {
                Recovering();
                RCD = 25;
            }
        }
    }

    public void ResetPlayerState() {
        this.Fireball.Cooldown = 2.25f;
        this.Dash.Cooldown = 4;
        this.Dash.Locked = true;
        this.Recover.Cooldown = 25f; //Spell Cooldown 25s
        this.GreenFireball.Cooldown = 10f;//Spell Cooldown 10s
        this.BulletTime.Cooldown = 60f;//Spell Cooldown 60s

        this.UpgradesCollection.Clear();

        this.Coins = 0;
        this.Level = 1;
    }

    public void AddCoins(int amountToAdd) {
        if (amountToAdd == 0) return;

        if (amountToAdd > 0 && HasUpgrade(PlayerUpgrade.SignetOfReplication)) {
            amountToAdd *= 2;
        }

        SFXPool.Instance.PlaySFX(SFXPool.SFX.Coin);

        this.Coins += amountToAdd;
        this.OnCoinsAdded?.Invoke(amountToAdd);
    }

    //helper code
    public void Recovering()
    {
        recoverying = !recoverying;
    }

}
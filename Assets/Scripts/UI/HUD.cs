using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] private Image m_HealthBarRenderer;
    [SerializeField] private TextMeshProUGUI m_CoinsAmount;
    [SerializeField] private TextMeshProUGUI m_CoinsAdded;
    [SerializeField] private Image m_FireballAbilityImage;
    [SerializeField] private Image m_DashAbilityImage;
    [SerializeField] private Image m_DashLockImage;
    [SerializeField] private Image m_PotionImage;
    [SerializeField] private TextMeshProUGUI m_PotionCount;

    [SerializeField] private Transform m_PotionTransform;
    [SerializeField] private Transform m_FireballTransform;
    [SerializeField] private Transform m_DashTransform;

    public Image FireballAbilityImage => m_FireballAbilityImage;
    public Image DashAbilityImage => m_DashAbilityImage;

    public Transform PotionTransform => m_PotionTransform;

    public Transform FireballTransform => m_FireballTransform;

    public Transform DashTransform => m_DashTransform;

    private void Start() {
        m_CoinsAdded.text = "";
        m_CoinsAmount.text = "" + PlayerState.Instance.Coins;

        m_DashLockImage.gameObject.SetActive(PlayerState.Instance.Dash.Locked);

        UpdatePotionGraphics();
    }

    public void UpdatePotionGraphics() {
        int potionCount = 0;
        if (PlayerState.Instance.UpgradesCollection.ContainsKey(PlayerState.PlayerUpgrade.HealingPotion)) {
            potionCount = PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.HealingPotion];
        }

        m_PotionCount.text = potionCount + "";
        m_PotionImage.fillAmount = potionCount > 0 ? 1 : 0;
    }

    public void UpdateHealthBarValue(float normalizedValue) {
        m_HealthBarRenderer.DOKill();
        m_HealthBarRenderer.DOFillAmount(normalizedValue, 0.5f);
    }

    public void UpdateCoinsAmount(int newCoinValue) {
        m_CoinsAmount.DOKill();
        m_CoinsAmount.DOText("" + newCoinValue, 0.5f, false, ScrambleMode.Numerals, "0123456789");
    }

    public void UpdateCoinsAdded(int coinsAdded) {
        m_CoinsAdded.DOKill();
        m_CoinsAdded.alpha = 0;
        m_CoinsAdded.DOFade(1, 0.5f);

        m_CoinsAdded.text = coinsAdded > 0 ? "+" + coinsAdded : coinsAdded + "";

        m_CoinsAdded.DOFade(0, 0.5f).SetDelay(1);
    }
}
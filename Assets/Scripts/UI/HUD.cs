using System;
using System.Collections;
using System.Collections.Generic;
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

    public Image FireballAbilityImage => m_FireballAbilityImage;
    public Image DashAbilityImage => m_DashAbilityImage;

    private void Start() {
        m_CoinsAdded.text = "";
        m_CoinsAmount.text = "" + PlayerState.Instance.Coins;

        m_DashLockImage.gameObject.SetActive(PlayerState.Instance.Dash.Locked);
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
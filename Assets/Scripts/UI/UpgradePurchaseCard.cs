using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePurchaseCard : MonoBehaviour {
    [SerializeField] private PlayerState.PlayerUpgrade m_PlayerUpgrade;
    [SerializeField] private bool m_CanBePurchasedAgain = false;
    [SerializeField] private Button m_PurchaseButton;

    [SerializeField] private int m_Cost;
    [SerializeField] private string m_Title;
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private string m_Description;

    [SerializeField] private TextMeshProUGUI m_NameLabel;
    [SerializeField] private Image m_IconImage;
    [SerializeField] private TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] private TextMeshProUGUI m_CostLabel;

    private void Start() {
        if (PlayerState.Instance.HasUpgrade(m_PlayerUpgrade) && !m_CanBePurchasedAgain) {
            gameObject.SetActive(false);
        }

        UpdatePurchasableState(PlayerState.Instance.Coins);

        m_NameLabel.text = m_Title;
        m_IconImage.sprite = m_Icon;
        m_DescriptionLabel.text = m_Description;
        m_CostLabel.text = "" + m_Cost;
    }

    private void OnEnable() {
        if (PlayerState.Instance != null)
            PlayerState.Instance.OnCoinsAmountChanged += UpdatePurchasableState;
        m_PurchaseButton.onClick.AddListener(Purchase);
    }

    private void OnDisable() {
        if (PlayerState.Instance != null)
            PlayerState.Instance.OnCoinsAmountChanged -= UpdatePurchasableState;
        m_PurchaseButton.onClick.RemoveListener(Purchase);
    }

    private void UpdatePurchasableState(int coinAmount) {
        if (coinAmount < m_Cost) {
            m_PurchaseButton.interactable = false;
        }
    }

    private void Purchase() {
        PlayerState.Instance.AddCoins(-m_Cost);
        PlayerState.Instance.AddUpgrade(m_PlayerUpgrade);
        m_PurchaseButton.gameObject.SetActive(false);
    }
}
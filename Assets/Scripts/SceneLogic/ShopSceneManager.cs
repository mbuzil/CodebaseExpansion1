using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopSceneManager : MonoBehaviour {
    [SerializeField] private CanvasGroup m_Fader;
    [SerializeField] private TextMeshProUGUI m_CoinAmountLabel;
    [SerializeField] private Transform m_RandomCardSection;
    
    private void Awake() {
        m_Fader.alpha = 1;
        m_Fader.DOFade(0, 0.25f);

        UpdateCoinAmount(PlayerState.Instance.Coins);

        int randomCardSelection = Random.Range(0, m_RandomCardSection.childCount);
        for (int i = 0; i < m_RandomCardSection.childCount; i++) {
            m_RandomCardSection.GetChild(i).gameObject.SetActive(i == randomCardSelection);
        }
    }

    private void OnEnable() {
        PlayerState.Instance.OnCoinsAmountChanged += UpdateCoinAmount;
    }

    private void OnDisable() {
        PlayerState.Instance.OnCoinsAmountChanged -= UpdateCoinAmount;
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            m_Fader.DOFade(1, 0.25f).OnComplete(() => { UnityEngine.SceneManagement.SceneManager.LoadScene(1); });
            PlayerState.Instance.Level++;
        }
    }

    private void UpdateCoinAmount(int coinAmount) {
        m_CoinAmountLabel.text = coinAmount + "";
    }
}
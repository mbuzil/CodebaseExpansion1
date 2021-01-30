using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using DG.Tweening;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class GamescreenOverlay : MonoBehaviour {
    [SerializeField] private CanvasGroup m_IntroScreen;
    [SerializeField] private CanvasGroup m_DeathScreen;
    [SerializeField] private CanvasGroup m_CompletionScreen;
    [SerializeField] private CanvasGroup m_FullScreenCover;

    private bool m_IntroScreenActive = false;
    private bool m_DeathScreenActive = false;
    private bool m_CompletionScreenActive = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (m_IntroScreenActive) {
                HideIntroScreen();
            }

            if (m_DeathScreenActive) {
                // Death routine
                LoadScene(0);
            }

            if (m_CompletionScreenActive) {
                // Completion routine
                LoadScene(0);
            }
        }
    }

    public void ShowIntroScreen() {
        Time.timeScale = 0;
        m_IntroScreenActive = true;
        m_IntroScreen.alpha = 1;

        m_FullScreenCover.alpha = 1;
        m_FullScreenCover.DOFade(0, 0.25f).SetUpdate(true);
    }

    private void HideIntroScreen() {
        Time.timeScale = 1;
        m_IntroScreenActive = false;
        m_IntroScreen.DOFade(0, 0.5f);
    }

    public void ShowDeathScreen() {
        Time.timeScale = 0;
        m_DeathScreenActive = true;
        m_DeathScreen.DOFade(1, 0.5f).SetUpdate(true);
    }

    public void ShowCompletionScreen() {
        Time.timeScale = 0;
        m_CompletionScreenActive = true;
        m_CompletionScreen.DOFade(1, 0.5f).SetUpdate(true);
    }

    private void LoadScene(int id) {
        m_IntroScreenActive = false;
        m_DeathScreenActive = false;
        m_CompletionScreenActive = false;

        m_FullScreenCover.DOFade(1, 0.25f).SetUpdate(true).OnComplete(
            () => UnityEngine.SceneManagement.SceneManager.LoadScene(id));
    }
}
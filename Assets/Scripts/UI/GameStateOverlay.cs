using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameStateOverlay : MonoBehaviour {
    [SerializeField] private CanvasGroup m_IntroScreen;
    [SerializeField] private CanvasGroup m_DeathScreen;
    [SerializeField] private CanvasGroup m_FullScreenCover;
    [SerializeField] private CanvasGroup m_LoreNoteScreen;
    [SerializeField] private TextMeshProUGUI m_LevelLabel;
    [SerializeField] private TextMeshProUGUI m_LoreNoteLabel;


    private bool m_IntroScreenActive = false;
    private bool m_DeathScreenActive = false;
    private bool m_LoreNoteScreenActive = false;

    [SerializeField] private string[] m_LoreNoteText = {
        "This is level 1",
        "This is level 2",
        "This is the last one I have.."
    };

    private void Start() {
        m_LevelLabel.text = "Level " + PlayerState.Instance.Level;
        m_LoreNoteLabel.text = m_LoreNoteText[Mathf.Min(PlayerState.Instance.Level - 1, m_LoreNoteText.Length - 1)];
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (m_IntroScreenActive) {
                HideIntroScreen();
            }

            if (m_DeathScreenActive) {
                // Death routine
                PlayerState.Instance?.ResetPlayerState();
                LoadScene(0);
            }

            if (m_LoreNoteScreenActive) {
                m_LoreNoteScreenActive = false;
                m_LoreNoteScreen.DOFade(0, 0.25f).SetUpdate(true).OnComplete(() => { Time.timeScale = 1; });
            }
        }
    }

    public void ShowLoreNoteScreen() {
        if (m_LoreNoteScreenActive) return;

        Time.timeScale = 0;

        m_LoreNoteScreenActive = true;

        m_LoreNoteScreen.alpha = 0;
        m_LoreNoteScreen.DOFade(1, 0.5f).SetUpdate(true);
    }


    public void ShowIntroScreen() {
        Time.timeScale = 0;
        m_IntroScreenActive = true;
        m_IntroScreen.alpha = 1;

        m_FullScreenCover.alpha = 1;
        m_FullScreenCover.DOFade(0, 0.25f).SetUpdate(true);
    }

    private void HideIntroScreen() {
        m_IntroScreenActive = false;
        m_IntroScreen.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => { Time.timeScale = 1; });
    }

    public void ShowDeathScreen() {
        StartCoroutine(ShowDeathScreenDelayedCoroutine());
    }

    private IEnumerator ShowDeathScreenDelayedCoroutine() {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        m_DeathScreenActive = true;
        m_DeathScreen.DOFade(1, 0.5f).SetUpdate(true);
    }

    public void CompleteLevel() {
        Time.timeScale = 0;
        LoadScene(2);
    }

    private void LoadScene(int id) {
        DOTween.KillAll();

        m_IntroScreenActive = false;
        m_DeathScreenActive = false;

        m_FullScreenCover.DOFade(1, 0.25f).SetUpdate(true).OnComplete(
            () => {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(id);
            });
    }
}
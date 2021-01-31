using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {
    [SerializeField] private AudioSource m_AudioSource;
    private void Awake() {
        StartCoroutine(DelayedSkip());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Skip();
        }
    }

    private IEnumerator DelayedSkip() {
        float timeScaleBefore = Time.timeScale; // remember for debugging purposes
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = timeScaleBefore;
        m_AudioSource.DOFade(0.4f, 0.5f);


        yield return new WaitForSeconds(23f);

        m_AudioSource.DOFade(0, 2);
        
        yield return new WaitForSeconds(2f);
        
        
        Skip();
    }

    private void Skip() {
        SceneManager.LoadScene(1);
    }
}
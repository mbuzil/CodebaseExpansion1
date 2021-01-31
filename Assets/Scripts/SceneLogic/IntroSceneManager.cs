using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {
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


        yield return new WaitForSeconds(25f);
        Skip();
    }

    private void Skip() {
        SceneManager.LoadScene(1);
    }
}
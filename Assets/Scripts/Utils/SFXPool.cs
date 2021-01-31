using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour {
    public enum SFX {
        SpellCast1,
        SpellImpact,
        Death1,
        HitImpact,
        Coin,
        Swoosh
    }

    [SerializeField] private List<SFX> m_SFXList = null;
    [SerializeField] private List<AudioClip> m_SFXClipsList = null;
    [SerializeField] private int m_InitialPoolSize = 10;

    public static SFXPool Instance;

    private List<AudioSource> m_SpawnedAudioSources = new List<AudioSource>();
    private Dictionary<SFX, AudioClip> m_SFXToClipsDictionary = new Dictionary<SFX, AudioClip>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < m_InitialPoolSize; i++) {
            m_SpawnedAudioSources.Add(SpawnAudioSource());
        }

        for (int i = 0; i < m_SFXList.Count; i++) {
            m_SFXToClipsDictionary.Add(m_SFXList[i], m_SFXClipsList[i]);
        }
    }

    public void PlaySFX(SFX sfx) {
        if (!m_SFXToClipsDictionary.ContainsKey(sfx)) {
            Debug.LogError("Missing SFX @ " + sfx);
            return;
        }

        StartCoroutine(PlaySFXCoroutine(sfx));
    }

    private IEnumerator PlaySFXCoroutine(SFX sfx) {
        if (m_SpawnedAudioSources.Count <= 0) {
            m_SpawnedAudioSources.Add(SpawnAudioSource());
        }

        AudioSource audioSource = m_SpawnedAudioSources[0];
        m_SpawnedAudioSources.RemoveAt(0);

        AudioClip clip = m_SFXToClipsDictionary[sfx];

        audioSource.clip = clip;
        audioSource.gameObject.SetActive(true);
        audioSource.Play();

        yield return new WaitForSeconds(clip.length + 0.5f);

        audioSource.gameObject.SetActive(false);
        m_SpawnedAudioSources.Add(audioSource);
    }

    private AudioSource SpawnAudioSource() {
        GameObject audioSourceGO = new GameObject("AudioSource");
        audioSourceGO.SetActive(false);
        audioSourceGO.transform.SetParent(transform);

        AudioSource audioSource = audioSourceGO.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        return audioSource;
    }
}
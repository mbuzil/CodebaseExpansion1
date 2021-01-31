using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour {
    // TODO: I'll make it into an actual pool if time allows

    public enum VFX {
        FireballPop,
        EnemyBookPop,
        EnemyGhostPop
    }

    [SerializeField] private List<VFX> m_VFXList = null;
    [SerializeField] private List<GameObject> m_VFXPrefabsList = null;

    public static VFXPool Instance;

    private Dictionary<VFX, GameObject> m_VFXToPrefabDictionary = new Dictionary<VFX, GameObject>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
            return;
        }


        for (int i = 0; i < m_VFXPrefabsList.Count; i++) {
            m_VFXToPrefabDictionary.Add(m_VFXList[i], m_VFXPrefabsList[i]);
        }
    }

    public void PlayVFX(VFX vfx, Vector3 position) {
        if (!m_VFXToPrefabDictionary.ContainsKey(vfx)) {
            Debug.LogError("Missing VFX @ " + vfx);
            return;
        }

        StartCoroutine(PlayVFXCoroutine(vfx, position));
    }

    private IEnumerator PlayVFXCoroutine(VFX vfx, Vector3 position) {
        GameObject spawnedPrefab = Instantiate(m_VFXToPrefabDictionary[vfx], transform);
        spawnedPrefab.transform.position = position;

        ParticleSystem particleSystem = spawnedPrefab.GetComponent<ParticleSystem>();
        particleSystem.Play(true);

        yield return new WaitForSeconds(particleSystem.main.duration + 0.5f);

        Destroy(spawnedPrefab);
    }
}
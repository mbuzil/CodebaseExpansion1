using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    [SerializeField] private List<GameObject> m_SpawnList;
    [SerializeField] [Range(0f, 1f)] private float m_SpawnChance;

    [Button]
    public void Spawn(int spawnIdOverride = -1) {
        Wipe();

        if (Random.Range(0, 100) < (int) ((m_SpawnChance) * 100 + (PlayerState.Instance.Level - 1) * 4)) {
            int spawnId = spawnIdOverride;
            if (spawnId < 0) {
                spawnId = Random.Range(0, m_SpawnList.Count);
            }

            GameObject spawnedGO = Instantiate(m_SpawnList[spawnId], transform);
            spawnedGO.transform.position = transform.position;
        }
    }

    [Button]
    private void Wipe() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
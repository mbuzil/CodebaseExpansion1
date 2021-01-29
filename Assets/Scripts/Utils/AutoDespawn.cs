using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour {
    [SerializeField] private float m_AutoDespawnTime = 3f;

    private float m_SpawnTimestamp = float.MaxValue;


    private void Awake() {
        m_SpawnTimestamp = Time.time;
    }

    private void Update() {
        if (m_AutoDespawnTime > 0 && m_SpawnTimestamp + m_AutoDespawnTime < Time.time) {
            Destroy(gameObject);
        }
    }
}
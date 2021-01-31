using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    private int[] m_Coefs = new int[4];

    public void Spawn() {
        m_Coefs[0] = 70;
        m_Coefs[1] = 50 + 1 * (PlayerState.Instance.Level - 1);
        m_Coefs[2] = (int) (20 + 2.5f * (PlayerState.Instance.Level - 1));
        m_Coefs[3] = 10 + 2 * (PlayerState.Instance.Level - 1);

        int roll = GetEnemyToSpawn(m_Coefs);

        GameObject toSpawn = null;

        switch (roll) {
            case 0:
                toSpawn = AssetDatabase.Instance.EnemySlimePrefab;
                break;
            case 1:
                toSpawn = AssetDatabase.Instance.EnemyBatPrefab;
                break;
            case 2:
                toSpawn = AssetDatabase.Instance.EnemyBookPrefab;
                break;
            case 3:
                toSpawn = AssetDatabase.Instance.EnemyGhostPrefab;
                break;
        }

        if (toSpawn != null) {
            GameObject spawnedPrefab = Instantiate(toSpawn, transform);
            spawnedPrefab.transform.localPosition = Vector3.zero;

            switch (roll) {
                case 0:
                    if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.AnticorrosiveLining)) {
                        spawnedPrefab.GetComponentInChildren<ProjectileCollision>().Damage /= 2;
                    }

                    break;
                case 1:
                    if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.GarlicVileNecklace)) {
                        spawnedPrefab.GetComponentInChildren<ProjectileCollision>().Damage /= 2;
                    }

                    break;
                case 2:
                    if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.VoidLens)) {
                        spawnedPrefab.GetComponentInChildren<ProjectileCollision>().Damage /= 2;
                    }


                    break;
                case 3:
                    if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.SoulMirror)) {
                        spawnedPrefab.GetComponentInChildren<ProjectileCollision>().Damage /= 2;
                    }


                    break;
            }
        }
    }

    private int GetEnemyToSpawn(int[] enemyCoefs) {
        int total = 0;
        foreach (int num in enemyCoefs) {
            total += num;
        }

        int randomSelection = Random.Range(0, total);

        int accum = enemyCoefs[0];
        for (int i = 0; i < enemyCoefs.Length - 1; i++) {
            if (randomSelection < accum) {
                return i;
            }

            accum += enemyCoefs[i + 1];
        }

        return enemyCoefs.Length - 1;
    }
}
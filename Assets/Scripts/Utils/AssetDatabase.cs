using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetDatabase", menuName = "ScriptableObjects/AssetDatabase", order = 1)]
public class AssetDatabase : SerializedScriptableObject {
    #region Singleton

    public static AssetDatabase Instance {
        get {
            if (m_Instance == null) {
                m_Instance = Resources.LoadAll<AssetDatabase>("").FirstOrDefault();
            }

            return m_Instance;
        }
    }

    private static AssetDatabase m_Instance;

    #endregion

    [SerializeField] public GameObject PlayerDirectionSign;
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] public GameObject PlayerExfil;

    // Enemies
    [SerializeField] public GameObject EnemySlimePrefab;
    [SerializeField] public GameObject EnemyBookPrefab;
    [SerializeField] public GameObject EnemyBatPrefab;
    [SerializeField] public GameObject EnemyGhostPrefab;

    [SerializeField] public GameObject DeadWizard;
}
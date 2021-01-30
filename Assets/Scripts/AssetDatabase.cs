using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetDatabase", menuName = "ScriptableObjects/AssetDatabase", order = 1)]
public class AssetDataBase : SerializedScriptableObject {
    #region Singleton

    public static AssetDataBase Instance {
        get {
            if (m_Instance == null) {
                m_Instance = Resources.LoadAll<AssetDataBase>("").FirstOrDefault();
            }

            return m_Instance;
        }
    }

    private static AssetDataBase m_Instance;

    #endregion

    [SerializeField] public GameObject PlayerDirectionSign;
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] public GameObject PlayerExfil;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlockMapGenerator : MonoBehaviour, IMapGenerator {
    [SerializeField] private GameObject m_BlockPrefab;

    public void Generate() {
        MapBlock mapBlock = Instantiate(m_BlockPrefab, transform).GetComponentInChildren<MapBlock>();

        mapBlock.StartingBlock = true;
        mapBlock.MainPathBlock = true;

        mapBlock.SpawnDirectionSign();

        mapBlock.FinishBlock = true;

        mapBlock.PopulateBlock();
    }
}
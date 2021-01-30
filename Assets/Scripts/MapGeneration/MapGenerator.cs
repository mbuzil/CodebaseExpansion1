using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : SerializedMonoBehaviour, IMapGenerator {
    [SerializeField] private int m_Width = 4;
    [SerializeField] private int m_Height = 4;

    [SerializeField] List<Vector2Int> m_Path = new List<Vector2Int>();
    [SerializeField] List<BlockDescriptor> m_PathRoomDescriptors = new List<BlockDescriptor>();

    [SerializeField] private MapBlock[,] m_Map = null;
    [SerializeField] private List<GameObject> m_MapBlockPrefabs = null;

    [SerializeField] private Dictionary<GameObject, MapBlock> m_PrefabsToMapBlockDictionary = null;

    private List<Vector2Int> possiblePathDirections = new List<Vector2Int>() {
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    [Button]
    public void Generate() {
        WipeMap();
        MapPrefabsToBlockObjects();

        GenerateMainPath();
        GenerateRoomLayout();
    }

    private void GenerateMainPath() {
        m_Path = new List<Vector2Int>();
        HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();
        Vector2Int startingPosition = new Vector2Int(Random.Range(0, m_Width), m_Height - 1);
        Vector2Int endPosition = new Vector2Int(Random.Range(0, m_Width), 0);

        m_Path.Add(startingPosition);
        visitedPositions.Add(startingPosition);
        Vector2Int nextPosition = startingPosition;

        while (nextPosition != endPosition) {
            if (nextPosition.y == 0) {
                // Last row - just head towards the exit
                if (nextPosition.x > endPosition.x) {
                    nextPosition.x--;
                } else if (nextPosition.x < endPosition.x) {
                    nextPosition.x++;
                }
            } else {
                // Above rows - randomly traverse the path
                possiblePathDirections.Shuffle();
                for (int i = 0; i < possiblePathDirections.Count; i++) {
                    Vector2Int possibleMove = nextPosition + possiblePathDirections[i];

                    if (possibleMove.x < 0 || possibleMove.x >= m_Width || possibleMove.y < 0 ||
                        possibleMove.y >= m_Height) continue;

                    if (!visitedPositions.Contains(possibleMove)) {
                        nextPosition = possibleMove;
                        break;
                    }
                }
            }

            m_Path.Add(nextPosition);
            visitedPositions.Add(nextPosition);
        }
    }

    private void GenerateRoomLayout() {
        m_Map = new MapBlock[m_Width, m_Height];
        m_PathRoomDescriptors = new List<BlockDescriptor>();

        // Spawn the rooms according to descriptors on the main path
        for (int i = 0; i < m_Path.Count; i++) {
            BlockDescriptor blockDescriptor = new BlockDescriptor();

            if (i > 0) {
                blockDescriptor.ConnectPathPositions(m_Path[i], m_Path[i - 1]);
            }

            if (i < m_Path.Count - 1) {
                blockDescriptor.ConnectPathPositions(m_Path[i], m_Path[i + 1]);
            }

            m_PathRoomDescriptors.Add(blockDescriptor);

            GameObject mapBlockPrefab = GetRandomMapBlockFromDescriptor(blockDescriptor);
            if (mapBlockPrefab == null) continue;

            MapBlock mapBlock = SpawnBlockAtPosition(mapBlockPrefab, m_Path[i].x, m_Path[i].y)
                .GetComponentInChildren<MapBlock>();

            if (i == 0) {
                mapBlock.StartingBlock = true;
            } else if (i == m_Path.Count - 1) {
                mapBlock.GetComponentInChildren<MapBlock>().FinishBlock = true;
            }

            mapBlock.MainPathBlock = true;

            // Spawn direction signs and position to point at the next room
            mapBlock.SpawnDirectionSign();
            mapBlock.DirectionSign?.RotateTowards(m_Path[i + 1] - m_Path[i]);

            m_Map[m_Path[i].x, m_Path[i].y] = mapBlock;
        }

        // Fill in the non-crucial (outside of the main path) blocks
        for (int x = 0; x < m_Width; x++) {
            for (int y = 0; y < m_Height; y++) {
                if (m_Map[x, y] == null) {
                    m_Map[x, y] = SpawnBlockAtPosition(m_MapBlockPrefabs[Random.Range(0, m_MapBlockPrefabs.Count)], x,
                        y).GetComponentInChildren<MapBlock>();
                }

                m_Map[x, y].PopulateBlock();
            }
        }
    }

    [Button]
    private void WipeMap() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private GameObject SpawnBlockAtPosition(GameObject prefab, int x, int y) {
        GameObject mapBlock = Instantiate(prefab, transform);
        mapBlock.transform.position = transform.position +
                                      new Vector3(x * MapBlock.MapBlockSize.x,
                                          y * MapBlock.MapBlockSize.y);

        return mapBlock;
    }

    private void MapPrefabsToBlockObjects() {
        m_PrefabsToMapBlockDictionary = new Dictionary<GameObject, MapBlock>();

        for (int i = 0; i < m_MapBlockPrefabs.Count; i++) {
            m_PrefabsToMapBlockDictionary.Add(m_MapBlockPrefabs[i],
                m_MapBlockPrefabs[i].GetComponentInChildren<MapBlock>());
        }
    }

    private GameObject GetRandomMapBlockFromDescriptor(BlockDescriptor blockDescriptor) {
        m_MapBlockPrefabs.Shuffle();
        for (int i = 0; i < m_MapBlockPrefabs.Count; i++) {
            BlockDescriptor selectedBlockDescriptor =
                m_PrefabsToMapBlockDictionary[m_MapBlockPrefabs[i]].BlockDescriptor;
            if (selectedBlockDescriptor.RoomDescriptorSuffices(blockDescriptor)) {
                return m_MapBlockPrefabs[i];
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;

        if (m_Path == null) return;

        for (int i = 0; i < m_Path.Count - 1; i++) {
            Gizmos.DrawLine(
                transform.position +
                new Vector3(m_Path[i].x * MapBlock.MapBlockSize.x,
                    m_Path[i].y * MapBlock.MapBlockSize.y),
                transform.position +
                new Vector3(m_Path[i + 1].x * MapBlock.MapBlockSize.x,
                    m_Path[i + 1].y * MapBlock.MapBlockSize.y));
        }
    }

   
}
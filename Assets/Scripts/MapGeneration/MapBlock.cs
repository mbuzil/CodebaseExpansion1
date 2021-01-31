using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapBlock : SerializedMonoBehaviour {
    [SerializeField] private BlockDescriptor m_BlockDescriptor;
    [SerializeField] private Transform m_PlayerSpawnLocation;
    [SerializeField] private Transform m_PlayerExfilLocation;
    [SerializeField] private Transform m_PlayerDirectionSignLocation;

    public static readonly Vector2 MapBlockSize = new Vector2(16, 16);

    public BlockDescriptor BlockDescriptor => m_BlockDescriptor;
    public bool StartingBlock { get; set; }
    public bool FinishBlock { get; set; }
    public bool MainPathBlock { get; set; }
    public DirectionSign DirectionSign { get; private set; } = null;

    private List<ObjectSpawner> ObjectSpawners {
        get { return m_ObjectSpawners ??= GetComponentsInChildren<ObjectSpawner>().ToList(); }
    }

    private List<EnemySpawner> EnemySpawners {
        get { return m_EnemySpawners ??= GetComponentsInChildren<EnemySpawner>().ToList(); }
    }

    private List<Collider2D> Walls {
        get { return m_Walls ??= GetComponentsInChildren<Collider2D>().ToList(); }
    }

    private List<ObjectSpawner> m_ObjectSpawners = null;

    private List<EnemySpawner> m_EnemySpawners = null;

    private List<Collider2D> m_Walls = null;

    public void PopulateBlock() {
        MarkVerticalWalls();

        SpawnPlayer();
        SpawnExfil();

        foreach (ObjectSpawner objectSpawner in this.ObjectSpawners) {
            objectSpawner.Spawn();
        }

        foreach (EnemySpawner enemySpawner in this.EnemySpawners) {
            enemySpawner.Spawn();
        }
    }

    public void SpawnDirectionSign() {
        if (this.MainPathBlock && !this.FinishBlock) {
            GameObject directionSign = Instantiate(AssetDatabase.Instance.PlayerDirectionSign, transform);
            directionSign.transform.position = m_PlayerDirectionSignLocation.position;
            this.DirectionSign = directionSign.GetComponentInChildren<DirectionSign>();
        }
    }

    private void MarkVerticalWalls() {
        foreach (Collider2D wall in this.Walls) {
            if (Mathf.Abs(wall.transform.localRotation.eulerAngles.z) > 45) {
                wall.gameObject.layer = 10;
            }
        }
    }

    private void SpawnPlayer() {
        if (this.StartingBlock) {
            GameObject player = Instantiate(AssetDatabase.Instance.PlayerPrefab, transform);
            player.transform.position = m_PlayerSpawnLocation.position;
        }
    }

    private void SpawnExfil() {
        if (this.FinishBlock) {
            GameObject playerExfil = Instantiate(AssetDatabase.Instance.PlayerExfil, transform);
            playerExfil.transform.position = m_PlayerExfilLocation.position;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + new Vector3(-MapBlockSize.x / 2, MapBlockSize.y / 2),
            transform.position + new Vector3(MapBlockSize.x / 2, MapBlockSize.y / 2));

        Gizmos.DrawLine(transform.position + new Vector3(MapBlockSize.x / 2, MapBlockSize.y / 2),
            transform.position + new Vector3(MapBlockSize.x / 2, -MapBlockSize.y / 2));

        Gizmos.DrawLine(transform.position + new Vector3(MapBlockSize.x / 2, -MapBlockSize.y / 2),
            transform.position + new Vector3(-MapBlockSize.x / 2, -MapBlockSize.y / 2));

        Gizmos.DrawLine(transform.position + new Vector3(-MapBlockSize.x / 2, -MapBlockSize.y / 2),
            transform.position + new Vector3(-MapBlockSize.x / 2, MapBlockSize.y / 2));

        if (m_BlockDescriptor.HasPathOnTheBottom) {
            Gizmos.DrawWireCube(transform.position + new Vector3(0, -MapBlockSize.y / 2.2f), Vector3.one);
        }

        if (m_BlockDescriptor.HasPathOnTheTop) {
            Gizmos.DrawWireCube(transform.position + new Vector3(0, MapBlockSize.y / 2.2f), Vector3.one);
        }

        if (m_BlockDescriptor.HasPathOnTheLeft) {
            Gizmos.DrawWireCube(transform.position + new Vector3(-MapBlockSize.y / 2.2f, 0), Vector3.one);
        }

        if (m_BlockDescriptor.HasPathOnTheRight) {
            Gizmos.DrawWireCube(transform.position + new Vector3(MapBlockSize.y / 2.2f, 0), Vector3.one);
        }
    }
}
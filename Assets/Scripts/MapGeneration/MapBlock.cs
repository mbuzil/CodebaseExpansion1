using System.Collections;
using System.Collections.Generic;
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


    public void PopulateBlock() {
        SpawnPlayer();
        SpawnExfil();
    }

    public void SpawnDirectionSign() {
        if (this.MainPathBlock && !this.FinishBlock) {
            GameObject directionSign = Instantiate(AssetDataBase.Instance.PlayerDirectionSign, transform);
            directionSign.transform.position = m_PlayerDirectionSignLocation.position;
            this.DirectionSign = directionSign.GetComponentInChildren<DirectionSign>();
        }
    }

    private void SpawnPlayer() {
        if (this.StartingBlock) {
            GameObject player = Instantiate(AssetDataBase.Instance.PlayerPrefab, transform);
            player.transform.position = m_PlayerSpawnLocation.position;
        }
    }

    private void SpawnExfil() {
        if (this.FinishBlock) {
            GameObject playerExfil = Instantiate(AssetDataBase.Instance.PlayerExfil, transform);
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
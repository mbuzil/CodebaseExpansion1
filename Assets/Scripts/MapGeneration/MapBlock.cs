using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapBlock : SerializedMonoBehaviour {
    [SerializeField] private BlockDescriptor m_BlockDescriptor;

    public static readonly Vector2 MapBlockSize = new Vector2(12, 12);

    public BlockDescriptor BlockDescriptor => m_BlockDescriptor;
    public bool StartingBlock { get; set; }
    public bool FinishBlock { get; set; }

    private void OnDrawGizmosSelected() {
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
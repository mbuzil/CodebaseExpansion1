using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSign : MonoBehaviour {
    [SerializeField] private SpriteRenderer m_DirectionRenderer;

    public void RotateTowards(Vector2 direction) {
        if (direction.x > 0) {
            m_DirectionRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (direction.x < 0) {
            m_DirectionRenderer.transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (direction.y > 0) {
            m_DirectionRenderer.transform.rotation = Quaternion.Euler(0, 0, 180);
        } else if (direction.y < 0) {
        }
    }
}
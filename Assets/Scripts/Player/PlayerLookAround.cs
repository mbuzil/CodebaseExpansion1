using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerLookAround : MonoBehaviour {
    [SerializeField] private float m_LookUpDownOffset = 2f;

    private void Update() {
        float deltaY = Input.GetAxis("Vertical");
        transform.localPosition = Vector3.up * deltaY * m_LookUpDownOffset;
    }
}
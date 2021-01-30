using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    private IMapGenerator MapGenerator {
        get { return m_MapGenerator ??= GetComponentInChildren<IMapGenerator>(); }
    }

    private PlayerController PlayerController {
        get { return m_PlayerController ??= GetComponentInChildren<PlayerController>(); }
    }

    private CinemachineVirtualCamera FollowPlayerCamera {
        get { return m_FollowPlayerCamera ??= GetComponentInChildren<CinemachineVirtualCamera>(); }
    }

    private GamescreenOverlay GamescreenOverlay {
        get { return m_GamescreenOverlay ??= GetComponentInChildren<GamescreenOverlay>(); }
    }

    private ExfilLocation ExfilLocation {
        get { return m_ExfilLocation ??= GetComponentInChildren<ExfilLocation>(); }
    }

    private IMapGenerator m_MapGenerator;

    private PlayerController m_PlayerController;

    private CinemachineVirtualCamera m_FollowPlayerCamera;

    private GamescreenOverlay m_GamescreenOverlay;

    private ExfilLocation m_ExfilLocation;

    private void Awake() {
        this.MapGenerator.Generate();
        this.FollowPlayerCamera.Follow = this.PlayerController.transform;
        this.GamescreenOverlay.ShowIntroScreen();

        this.PlayerController.GetComponent<Damageable>().OnDied += this.GamescreenOverlay.ShowDeathScreen;
        this.ExfilLocation.OnExfiled += this.GamescreenOverlay.ShowCompletionScreen;
    }
}
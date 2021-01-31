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

    private PlayerLookAround PlayerLookAround {
        get { return m_PlayerLookAround ??= GetComponentInChildren<PlayerLookAround>(); }
    }

    private CinemachineVirtualCamera FollowPlayerCamera {
        get { return m_FollowPlayerCamera ??= GetComponentInChildren<CinemachineVirtualCamera>(); }
    }

    private GameStateOverlay GameStateOverlay {
        get { return m_GameStateOverlay ??= GetComponentInChildren<GameStateOverlay>(); }
    }

    private ExfilLocation ExfilLocation {
        get { return m_ExfilLocation ??= GetComponentInChildren<ExfilLocation>(); }
    }

    private HUD HUD {
        get { return m_HUD ??= GetComponentInChildren<HUD>(); }
    }

    private IMapGenerator m_MapGenerator;

    private PlayerController m_PlayerController;

    private PlayerLookAround m_PlayerLookAround;

    private CinemachineVirtualCamera m_FollowPlayerCamera;

    private GameStateOverlay m_GameStateOverlay;

    private ExfilLocation m_ExfilLocation;

    private HUD m_HUD;

    private void Awake() {
        this.MapGenerator.Generate();
        this.FollowPlayerCamera.Follow = this.PlayerLookAround.transform;
        this.GameStateOverlay.ShowIntroScreen();

        Damageable playerDamageable = this.PlayerController.GetComponent<Damageable>();
        playerDamageable.OnDied += this.GameStateOverlay.ShowDeathScreen;
        playerDamageable.OnCurrentHealthChanged += this.HUD.UpdateHealthBarValue;

        PlayerState.Instance.OnCoinsAdded += this.HUD.UpdateCoinsAdded;
        PlayerState.Instance.OnCoinsAmountChanged += this.HUD.UpdateCoinsAmount;

        PlayerState.Instance.Fireball.AbilityImage = this.HUD.FireballAbilityImage;
        PlayerState.Instance.Dash.AbilityImage = this.HUD.DashAbilityImage;

        this.ExfilLocation.OnExfiled += this.GameStateOverlay.ShowCompletionScreen;
    }
}
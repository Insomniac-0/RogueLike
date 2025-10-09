using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public unsafe class GameManager : MonoBehaviour
{
    [SerializeField] PlayerDataSO _base_stats_template;


    public bool PlayMode;
    public enum GameState
    {
        PLAY,
        PAUSE,
        UPGRADE,
        DEATH,
    }

    private GameState _current_state;
    private GameState _prev_state;
    private PlayerBaseStats base_stats;

    void Awake()
    {
        InitializeBaseStats();
    }

    void Start()
    {
        InitResources.GetInputReader.inputs.GeneralActions.Pause.performed += _ => PauseSwitch();
        InitResources.GetEventChannel.OnLvlUp += UpgradeState;
        InitResources.GetEventChannel.OnDeath += DeathState;
        PlayMode = false;
    }

    private void Update()
    {
        if (!PlayMode) return;


        switch (_current_state)
        {
            case GameState.PLAY:
                PlayUpdateLoop();
                break;
            case GameState.PAUSE:
                PauseUpdateLoop();
                break;
            case GameState.UPGRADE:
                UpgradeLoop();
                break;
            case GameState.DEATH:
                DeathLoop();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!PlayMode) return;
        switch (_current_state)
        {
            case GameState.PLAY:
                PlayFixedUpdateLoop();
                break;
            case GameState.PAUSE:
                break;
            case GameState.UPGRADE:
                break;
            case GameState.DEATH:
                break;
        }
    }

    public void StateInit(GameState state)
    {
        switch (state)
        {
            case GameState.PLAY:
                PlayStateInit();
                break;
            case GameState.PAUSE:
                PauseStateInit();
                break;
            case GameState.UPGRADE:
                UpgradeStateInit();
                break;
            case GameState.DEATH:
                DeathStateInit();
                break;
        }
    }



    public void InitializePlayerStats(ref PlayerStats p)
    {
        p.max_hp = base_stats.max_health;
        p.current_hp = base_stats.max_health;
        p.move_speed = base_stats.move_speed;
        p.attack_speed = base_stats.attack_speed;
    }

    public void SwitchState(GameState state)
    {
        if (_current_state != state)
        {
            _current_state = state;
            StateInit(state);
        }
    }

    public void UpgradeState() => SwitchState(GameState.UPGRADE);
    public void DeathState() => SwitchState(GameState.DEATH);

    public void SetState(GameState state) => _current_state = state;

    private void PlayUpdateLoop()
    {
        InitResources.GetPlayer.PlayerUpdate();
        NullableObjects.GetSpawner.UpdateSpawner();
    }
    private void PlayFixedUpdateLoop()
    {
        InitResources.GetProjectileManager.FixedProjectileUpdate();
        InitResources.GetEnemyManagerBehaviour.FixedEnemyUpdate();

    }
    private void PauseUpdateLoop()
    {
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
        NullableObjects.GetPlayer.SetVelocity(float3.zero);
    }

    private void UpgradeLoop()
    {
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
        NullableObjects.GetPlayer.SetVelocity(float3.zero);
    }

    private void DeathLoop()
    {
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
        NullableObjects.GetPlayer.SetVelocity(float3.zero);
    }

    void PauseSwitch()
    {
        if (_current_state != GameState.PAUSE)
        {
            _prev_state = _current_state;
            _current_state = GameState.PAUSE;
            StateInit(GameState.PAUSE);
        }
        else
        {
            _current_state = _prev_state;
            StateInit(_current_state);
        }


    }

    private void PlayStateInit()
    {
        Cursor.visible = false;
        InitResources.GetInputReader.inputs.GeneralActions.Enable();

        if (NullableObjects.GetUpgradeUI != null) NullableObjects.GetUpgradeUI.gameObject.SetActive(false);
        if (NullableObjects.GetPauseUI != null) NullableObjects.GetPauseUI.gameObject.SetActive(false);
        if (NullableObjects.GetGameOverUI != null) NullableObjects.GetGameOverUI.gameObject.SetActive(false);
    }

    private void PauseStateInit()
    {
        Cursor.visible = true;
        NullableObjects.GetPauseUI.gameObject.SetActive(true);
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
        NullableObjects.GetPlayer.SetVelocity(float3.zero);
    }

    private void UpgradeStateInit()
    {

        Cursor.visible = true;
        if (NullableObjects.GetUpgradeUI != null) NullableObjects.GetUpgradeUI.gameObject.SetActive(true);
        if (NullableObjects.GetPauseUI != null) NullableObjects.GetPauseUI.gameObject.SetActive(false);
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
    }

    private void DeathStateInit()
    {
        Cursor.visible = true;
        InitResources.GetInputReader.inputs.GeneralActions.Disable();
        NullableObjects.GetGameOverUI.gameObject.SetActive(true);
        InitResources.GetEnemyManagerBehaviour.PauseEnemies();
        NullableObjects.GetPlayer.SetVelocity(float3.zero);

        int score = InitResources.GetUpgradeSystem.GetScore;
        int kills = InitResources.GetUpgradeSystem.GetKills;

        InitResources.GetSaveManager.SaveScore(score, kills);

    }

    private void InitializeBaseStats()
    {
        base_stats.max_health = _base_stats_template.MaxHealth;
        base_stats.move_speed = _base_stats_template.MovementSpeed;
        base_stats.attack_speed = _base_stats_template.AttackSpeed;
        base_stats.pickup_range = _base_stats_template.PickUpRange;
    }
}

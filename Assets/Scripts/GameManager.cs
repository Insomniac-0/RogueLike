using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PLAY,
        PAUSE,
        UPGRADE,
        DEATH,
    }

    #region Singleton
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception("Singletons of type [GameManager] exist");
        }
        Instance = this;
    }
    #endregion


    private GameState _current_state;

    private void Update()
    {
        switch (_current_state)
        {
            case GameState.PLAY:
                break;
            case GameState.PAUSE:
                break;
            case GameState.UPGRADE:
                break;
            case GameState.DEATH:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (_current_state)
        {
            case GameState.PLAY:
                PlayUpdateLoop();
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

    public void SwitchState(GameState state)
    {
        _current_state = state;
    }

    private void PlayUpdateLoop()
    {
        
    }
    private void PlayFixedUpdateLoop()
    {

    }
    private void PauseUpdateLoop()
    {

    }
}

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

    private GameState _current_state;

    void Start()
    {
    }

    private void Update()
    {
        switch (_current_state)
        {
            case GameState.PLAY:
                PlayUpdateLoop();
                break;
            case GameState.PAUSE:
                PauseUpdateLoop();
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

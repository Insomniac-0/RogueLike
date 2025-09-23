using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action LVL_UP;
    private float _max_xp;
    private float _current_xp;

    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception("Singletons of type [GameManager] exist");
        }
        Instance = this;
        _current_xp = 0;
        _max_xp = 100;
    }
    #endregion



    public enum GameState
    {
        PLAY,
        PAUSE,
        UPGRADE,
        DEATH,
    }
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

    public void SwitchState(GameState state)
    {
        _current_state = state;
    }

    public void AddExperience(int XP = 1)
    {
        _current_xp += XP;
        if (_current_xp >= _max_xp)
        {
            LVL_UP.Invoke();
            _current_xp -= _max_xp;
            _max_xp *= 1.20f;
        }
    }

}

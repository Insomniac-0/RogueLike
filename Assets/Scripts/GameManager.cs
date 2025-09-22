using UnityEngine;

public class GameManager : MonoBehaviour
{
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

}

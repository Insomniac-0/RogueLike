using System;
using UnityEngine;

public class EventChannel : MonoBehaviour
{
    // PLAYER EVENTS
    public event Action OnHealthChange;
    public event Action OnXpChange;
    public event Action OnLvlUp;
    public event Action OnScoreChange;

    // GAME EVENTS
    public event Action OnExitGame;
    public event Action OnPlayGame;



    // PLAYER EVENTS
    public void TriggerHealthChange() => OnHealthChange?.Invoke();
    public void TriggerXpChange() => OnXpChange?.Invoke();
    public void TriggerLvlUp() => OnLvlUp?.Invoke();
    public void TriggerScoreChange() => OnScoreChange?.Invoke();

    // GAME EVENTS
    public void TriggerOnExitGame() => OnExitGame?.Invoke();
    public void TriggerOnPlayGame() => OnPlayGame?.Invoke();
}


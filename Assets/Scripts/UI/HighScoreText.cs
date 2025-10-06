using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    Text highscore_text;

    void Awake()
    {
        highscore_text = GetComponent<Text>();
    }

    void Start()
    {
        InitResources.GetEventChannel.OnScoreChange += UpdateHighScore;
    }

    void UpdateHighScore()
    {
        highscore_text.text = $"{InitResources.GetUpgradeSystem.GetScore}";
    }
}

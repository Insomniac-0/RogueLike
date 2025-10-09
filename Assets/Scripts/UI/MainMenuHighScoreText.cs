using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHighScoreText : MonoBehaviour
{
    [SerializeField] int width;

    Text text;
    SaveData data;
    const string divider = "==============\n";

    void Awake()
    {
        data = InitResources.GetSaveManager.GetSaveData;
        text = GetComponent<Text>();
        SetText();
    }
    void Start()
    {
        InitResources.GetEventChannel.OnNewHighScore += SetText;
    }

    void SetText()
    {
        text.text = $"{"Score",-4} | {"Kills",4}\n{divider}{data.HighScore,-4}|{data.EnemiesKilled,4}";
    }
}

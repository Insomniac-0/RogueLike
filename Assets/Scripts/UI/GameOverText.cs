using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{

    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        InitResources.GetEventChannel.OnDeath += SetText;
    }

    void SetText()
    {
        int score = InitResources.GetUpgradeSystem.GetScore;
        int kills = InitResources.GetUpgradeSystem.GetKills;
        text.text = $"Score - {score}\nKills - {kills}";
    }
}

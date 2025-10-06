using UnityEngine;
using UnityEngine.UI;

public class PlayerLvlText : MonoBehaviour
{
    Text lvl_text;

    void Awake()
    {
        lvl_text = GetComponent<Text>();
    }

    void Start()
    {
        InitResources.GetEventChannel.OnLvlUp += UpdateLvlText;
    }

    void UpdateLvlText()
    {
        lvl_text.text = $"{InitResources.GetUpgradeSystem.GetPlayerLvl}";
    }
}

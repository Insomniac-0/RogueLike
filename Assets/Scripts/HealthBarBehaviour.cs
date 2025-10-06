using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        InitResources.GetEventChannel.OnHealthChange += UpdateHealthBar;
    }

    void UpdateHealthBar()
    {
        image.fillAmount = math.clamp(InitResources.GetPlayer.GetHpPercentage, 0f, 1f);
    }
}

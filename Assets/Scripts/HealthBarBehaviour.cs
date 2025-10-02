using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    Player player;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (player != null)
        {
            image.fillAmount = math.clamp(InitResources.GetPlayer.GetHealthPercentage, 0f, 1f);
        }

    }
}

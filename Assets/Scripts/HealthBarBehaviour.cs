using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (InitResources.GetPlayer != null)
        {
            image.fillAmount = math.clamp(InitResources.GetPlayer.GetHpPercentage, 0f, 1f);
        }

    }
}

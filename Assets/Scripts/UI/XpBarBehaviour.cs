using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class XpBarBehaviour : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount = math.clamp(InitResources.GetUpgradeSystem.GetXpPercentage, 0f, 1f);
    }
}

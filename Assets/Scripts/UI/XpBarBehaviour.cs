using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class XpBarBehaviour : MonoBehaviour
{
    Player player;
    Image image;

    void Start()
    {
        player = InitResources.GetPlayer;
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (player != null)
        {
            image.fillAmount = math.clamp(InitResources.GetUpgradeSystem.GetXpPercentage, 0f, 1f);
        }
    }
}

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] Player player;

    void Update()
    {
        GetComponent<Image>().fillAmount = math.clamp(player.Health / 100f, 0f, 1f);
    }
}

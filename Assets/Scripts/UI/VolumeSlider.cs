using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        InitResources.GetNullableObjects.AssignVolumeSlider(slider);
        slider.value = InitResources.GetGameSettings.GetVolume;
    }
}

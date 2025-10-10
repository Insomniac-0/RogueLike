using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private float volume;
    private bool fullscreen;

    public bool GetFullScreen => fullscreen;
    public float GetVolume => volume;

    public void SetFullScreen(bool b)
    {
        if (b)
        {
            Screen.SetResolution(1920, 1080, b);
        }
        else
        {
            Screen.SetResolution(800, 600, b);
        }
    }


    public void SetVolume(float f) => volume = f;
    public void SetVsync(bool vsync) => QualitySettings.vSyncCount = vsync ? 1 : -1;


    void Awake()
    {
        volume = 0f;
    }
    void Start()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = -1;
    }
}

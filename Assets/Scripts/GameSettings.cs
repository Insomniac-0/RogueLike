using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private float volume;
    private bool fullscreen;

    public bool GetFullScreen => fullscreen;
    public float GetVolume => volume;

    public void SetFullScreen(bool b) => fullscreen = b;
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

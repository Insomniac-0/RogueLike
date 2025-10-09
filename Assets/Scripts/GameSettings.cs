using Unity.VisualScripting;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private float volume;
    private bool vsync;
    private bool fullscreen;

    public bool GetFullScreen => fullscreen;
    public bool GetVsync => vsync;
    public float GetVolume => volume;

    public void SetFullScreen(bool b) => fullscreen = b;
    public void SetVsync(bool b) => vsync = b;
    public void SetVolume(float f) => volume = f;

    void Awake()
    {
        volume = 0f;
        vsync = false;
    }
}

using System;
using JetBrains.Annotations;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    GameObject[] menus;
    const int size = 3;

    public enum Menu
    {
        DEFAULT,
        HIGHSCORE,
        OPTIONS,
    }

    void Awake()
    {
        menus = new GameObject[size];
    }

    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            menus[i] = transform.GetChild(i).gameObject;
        }
    }




    public void ActivateMenu(int m)
    {
        switch ((Menu)m)
        {
            case Menu.DEFAULT:
                menus[0].SetActive(true);
                menus[1].SetActive(false);
                menus[2].SetActive(false);
                break;
            case Menu.HIGHSCORE:
                menus[1].SetActive(true);
                menus[0].SetActive(false);
                menus[2].SetActive(false);
                break;
            case Menu.OPTIONS:
                menus[2].SetActive(true);
                menus[1].SetActive(false);
                menus[0].SetActive(false);
                break;
        }
    }


    public void SetFullscreen(bool b)
    {
        InitResources.GetGameSettings.SetFullScreen(b);
        InitResources.GetEventChannel.TriggerOnSettingsChange();
    }
    public void SetVolume(float f)
    {
        InitResources.GetGameSettings.SetVolume(f);
        InitResources.GetEventChannel.TriggerOnSettingsChange();
    }
    public void SetVsync(bool b)
    {
        InitResources.GetGameSettings.SetVsync(b);
        InitResources.GetEventChannel.TriggerOnSettingsChange();
    }
}

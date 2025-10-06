using System;
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

    Menu current;

    void Awake()
    {
        menus = new GameObject[size];
        current = Menu.DEFAULT;
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
}

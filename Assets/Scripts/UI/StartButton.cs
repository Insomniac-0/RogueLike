using System;
using System.IO.Compression;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button button;
    void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SelectGameScene()
    {

        Debug.Log("SelectGameClicked");

    }
}

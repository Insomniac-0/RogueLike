using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    void Awake()
    {
        Cursor.visible = true;
    }
    public void StartGame()
    {
        InitResources.Init();
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {

    }
}

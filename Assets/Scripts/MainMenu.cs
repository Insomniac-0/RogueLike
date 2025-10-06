using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = true;
    }
    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(2);
    }
}

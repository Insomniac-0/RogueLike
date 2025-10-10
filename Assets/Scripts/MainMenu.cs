using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SoundData MenuMusic;
    PrematureDespawnTrigger StopMenuMusic;
    void Awake()
    {
        Cursor.visible = true;
        StopMenuMusic = InitResources.GetSoundManager.SpawnSound(MenuMusic);
    }
    public void StartGame()
    {
        StopMenuMusic();
        InitResources.Init();
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

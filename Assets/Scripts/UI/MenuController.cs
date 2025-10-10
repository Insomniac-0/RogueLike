using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] SoundData GameMusic;
    PrematureDespawnTrigger StopGameMusic;

    void Awake()
    {

    }
    void Start()
    {
        StopGameMusic = InitResources.GetSoundManager.SpawnSound(GameMusic);
        InitResources.GetGameManager.PlayMode = true;
        InitResources.GetGameManager.SetState(GameManager.GameState.PLAY);
        InitResources.GetGameManager.StateInit(GameManager.GameState.PLAY);
    }
    public void UpgradeMoveSpeed() => InitResources.GetUpgradeSystem.UpgradeMovementSpeed();
    public void UpgradeHealth() => InitResources.GetUpgradeSystem.UpgradeMaxHealth();
    public void UpgradeDMG() => InitResources.GetUpgradeSystem.UpgradeDamage();
    public void UpgradeAS() => InitResources.GetUpgradeSystem.UpgradeFireRate();

    public void EnterPlayState()
    {
        InitResources.GetGameManager.SwitchState(GameManager.GameState.PLAY);
    }

    public void PlayAgain()
    {
        InitResources.GetInputReader.inputs.GeneralActions.Enable();
        InitResources.SoftCleanUp();
        InitResources.Init();
        InitResources.GetPlayer.GetPlayerBehaviour.Reset();
        InitResources.GetGameManager.SwitchState(GameManager.GameState.PLAY);
    }
    public void ExitGame()
    {

        StopGameMusic();
        InitResources.GetGameManager.PlayMode = false;
        InitResources.GetInputReader.inputs.GeneralActions.Disable();
        InitResources.HardCleanUp();
        SceneManager.LoadSceneAsync(1);
    }
}

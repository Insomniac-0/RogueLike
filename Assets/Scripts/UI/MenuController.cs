using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        InitResources.GetGameManager.PlayMode = true;
        InitResources.GetGameManager.SetState(GameManager.GameState.PLAY);
        InitResources.GetGameManager.StateInit(GameManager.GameState.PLAY);
    }
    public void UpgradeMoveSpeed() => InitResources.GetUpgradeSystem.UpgradeMovementSpeed();
    public void UpgradeHealth() => InitResources.GetUpgradeSystem.UpgradeMaxHealth();
    public void UpgradeDMG() => InitResources.GetUpgradeSystem.UpgradeDamage();

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
        InitResources.GetGameManager.PlayMode = false;
        InitResources.GetInputReader.inputs.GeneralActions.Disable();
        InitResources.HardCleanUp();
        SceneManager.LoadSceneAsync(1);

    }
}

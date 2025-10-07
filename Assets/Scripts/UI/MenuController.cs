using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Awake()
    {
        InitResources.GetGameManager.PlayMode = true;
        InitResources.GetGameManager.SetState(GameManager.GameState.PLAY);
        InitResources.GetGameManager.StateInit(GameManager.GameState.PLAY);
    }
    public void UpgradeMoveSpeed() => InitResources.GetUpgradeSystem.UpgradeMovementSpeed();
    public void UpgradeHealth() => InitResources.GetUpgradeSystem.UpgradeMaxHealth();
    public void UpgradeDMG() => InitResources.GetUpgradeSystem.UpgradeDamage();

    public void EnterPlayState() => InitResources.GetGameManager.SwitchState(GameManager.GameState.PLAY);

    public void ExitGame()
    {
        InitResources.GetGameManager.PlayMode = false;
        InitResources.GetProjectileManager.CleanUp();
        InitResources.GetEnemyManagerBehaviour.CleanUp();
        InitResources.GetVfxManager.CleanUp();
        SceneManager.LoadSceneAsync(1);

    }
}

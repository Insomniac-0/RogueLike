using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    int HighScore;
    int EnemiesKilled;

    Upgrade[] upgrades;


    public struct PlayerLvl
    {
        public int lvl;
        public float max_xp;
        public float current_xp;
    }



    PlayerLvl player_lvl;


    public void Init()
    {
        player_lvl.lvl = 1;
        player_lvl.max_xp = 100f;
        player_lvl.current_xp = 0f;

        HighScore = 0;
        EnemiesKilled = 0;
        InitResources.GetEventChannel.TriggerXpChange();
        InitResources.GetEventChannel.TriggerLvlUp();
        InitResources.GetEventChannel.TriggerScoreChange();
    }

    public void AddExperience(float xp)
    {
        player_lvl.current_xp += xp;

        if (player_lvl.current_xp >= player_lvl.max_xp)
        {
            player_lvl.current_xp -= player_lvl.max_xp;
            player_lvl.max_xp *= 1.1f;
            player_lvl.lvl++;
            InitResources.GetEventChannel.TriggerLvlUp();
        }

        InitResources.GetEventChannel.TriggerXpChange();
    }

    public void UpgradeMovementSpeed_M(float f)
    {
        PlayerMultipliers m = InitResources.GetPlayer.GetPlayerBehaviour.GetMultipliers;
        m.ms_multiply *= f;
        InitResources.GetPlayer.GetPlayerBehaviour.SetMultipliers(m);
        InitResources.GetPlayer.GetPlayerBehaviour.UpdateMoveSpeed();
    }
    public void UpgradeMovementSpeed() => UpgradeMovementSpeed_M(1.2f);

    public void UpgradeMaxHealth_A(float f)
    {
        InitResources.GetPlayer.GetPlayerBehaviour.IncreaseMaxHP(f);
        InitResources.GetPlayer.GetPlayerBehaviour.IncreaseCurrentHP(f);
        InitResources.GetEventChannel.TriggerHealthChange();
    }

    public void UpgradeFireRate_M(float f)
    {
        PlayerMultipliers m = InitResources.GetPlayer.GetPlayerBehaviour.GetMultipliers;
        m.as_multiply *= f;
        InitResources.GetPlayer.GetPlayerBehaviour.SetMultipliers(m);
        InitResources.GetPlayer.GetPlayerBehaviour.UpdateAttackSpeed();
    }

    public void UpgradeFireRate() => UpgradeFireRate_M(1.2f);

    public void UpgradeMaxHealth() => UpgradeMaxHealth_A(10f);

    public void UpgradeDamage_M(float f)
    {
        PlayerMultipliers m = InitResources.GetPlayer.GetPlayerBehaviour.GetMultipliers;
        m.dmg_multiply *= f;
        InitResources.GetPlayer.GetPlayerBehaviour.SetMultipliers(m);
    }
    public void UpgradeDamage() => UpgradeDamage_M(1.2f);

    public void AddScore(int score) => HighScore += score;
    public void UpdateKills() => EnemiesKilled++;


    public float GetXpPercentage => player_lvl.current_xp / player_lvl.max_xp;
    public int GetPlayerLvl => player_lvl.lvl;
    public int GetScore => HighScore;
    public int GetKills => EnemiesKilled;

}

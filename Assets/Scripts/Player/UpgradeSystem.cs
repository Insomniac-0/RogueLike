using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{

    int HighScore;

    public struct PlayerLvl
    {
        public int lvl;
        public float max_xp;
        public float current_xp;
    }

    public struct Upgrade
    {
        public ScalingType scaling_type;
        float value;
    }


    PlayerLvl player_lvl;


    public void Init()
    {
        player_lvl.lvl = 1;
        player_lvl.max_xp = 100f;
        player_lvl.current_xp = 0f;

        HighScore = 0;
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

    public void UpgradeMovementSpeed()
    {
        PlayerMultipliers m = InitResources.GetPlayer.GetPlayerBehaviour.GetMultipliers;
        m.ms_multiply *= 1.2f;
        InitResources.GetPlayer.GetPlayerBehaviour.SetMultipliers(m);
        InitResources.GetPlayer.GetPlayerBehaviour.UpdateMoveSpeed();
    }

    public void UpgradeMaxHealth()
    {
        InitResources.GetPlayer.GetPlayerBehaviour.IncreaseMaxHP(10);
        InitResources.GetPlayer.GetPlayerBehaviour.IncreaseCurrentHP(10);
        InitResources.GetEventChannel.TriggerHealthChange();
    }

    public void UpgradeDamage()
    {
        PlayerMultipliers m = InitResources.GetPlayer.GetPlayerBehaviour.GetMultipliers;
        m.dmg_multiply *= 1.2f;
        InitResources.GetPlayer.GetPlayerBehaviour.SetMultipliers(m);
    }

    public void AddScore(int score) => HighScore += score;

    public float GetXpPercentage => player_lvl.current_xp / player_lvl.max_xp;
    public int GetPlayerLvl => player_lvl.lvl;
    public int GetScore => HighScore;

}

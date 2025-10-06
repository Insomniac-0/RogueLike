using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UpgradeSystem : MonoBehaviour
{
    PlayerBehaviour player_behaviour;

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

    void Awake()
    {
        player_behaviour = GetComponent<PlayerBehaviour>();
        player_lvl.lvl = 1;
        player_lvl.max_xp = 100f;
        player_lvl.current_xp = 0f;

        HighScore = 0;
    }

    public void AddExperience(float xp)
    {
        player_lvl.current_xp += xp;

        if (player_lvl.current_xp >= player_lvl.max_xp)
        {
            player_lvl.current_xp -= player_lvl.max_xp;
            player_lvl.max_xp *= 1.2f;
            player_lvl.lvl++;
            InitResources.GetEventChannel.TriggerLvlUp();
        }

        InitResources.GetEventChannel.TriggerXpChange();
    }

    public void UpgradeMovementSpeed()
    {
        player_behaviour.player_stats.ms_multiply *= 1.2f;
        player_behaviour.player_data.speed =
            player_behaviour.player_stats.base_move_speed * player_behaviour.player_stats.ms_multiply;

    }

    public void UpgradeMaxHealth()
    {
        player_behaviour.player_stats.max_health += 10f;
        player_behaviour.player_stats.current_hp += 10f;
        InitResources.GetEventChannel.TriggerHealthChange();
    }

    public void UpgradeDamage()
    {
        player_behaviour.player_stats.dmg_multiply += 0.2f;
    }

    public void AddScore(int score) => HighScore += score;

    public float GetXpPercentage => player_lvl.current_xp / player_lvl.max_xp;
    public int GetPlayerLvl => player_lvl.lvl;
    public int GetScore => HighScore;

}

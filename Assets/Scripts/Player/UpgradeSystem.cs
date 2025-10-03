using System;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public event Action OnLevelUp;
    PlayerBehaviour player_behaviour;

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

    void Awake()
    {
        player_behaviour = GetComponent<PlayerBehaviour>();
        player_lvl.max_xp = 100f;
        player_lvl.current_xp = 0f;
    }


    PlayerLvl player_lvl;



    public void AddExperience(float xp)
    {
        player_lvl.current_xp += xp;
        if (player_lvl.current_xp >= player_lvl.max_xp)
        {
            player_lvl.current_xp -= player_lvl.max_xp;
            player_lvl.max_xp *= 1.2f;
            OnLevelUp?.Invoke();
        }
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
    }

    public void UpgradeDamage()
    {
        player_behaviour.player_stats.dmg_multiply += 0.2f;
    }

    public float GetXpPercentage => player_lvl.current_xp / player_lvl.max_xp;
}

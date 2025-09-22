using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float _max_health;
    [SerializeField] private float _movement_speed;

    public float MaxHealth;
    public float MovementSpeed;
}

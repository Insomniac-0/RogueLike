using System.Data.Common;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float _max_health;
    [SerializeField] private float _movement_speed;

    public float MaxHealth => _max_health;
    public float MovementSpeed => _max_health;
}


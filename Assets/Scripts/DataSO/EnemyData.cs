using System.Data.Common;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float MaxHealth;
    public float BaseMovementSpeed;
    public float BaseDMG;
}


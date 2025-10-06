using System.Data.Common;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public float MaxHealth;
    public float BaseMovementSpeed;
    public float CrawlSpeed;
    public float Range;
    public float AttackRange;
    public float AttackDelay;
    public float BaseDMG;
}


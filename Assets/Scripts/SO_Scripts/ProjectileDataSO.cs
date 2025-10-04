using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
public class ProjectileDataSO : ScriptableObject
{
    public Projectile prefab;

    public int BaseHP;
    public float Lifetime;
    public float BaseSpeed;
    public float BaseDMG;
}

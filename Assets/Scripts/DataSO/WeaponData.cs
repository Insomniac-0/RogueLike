using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int ProjectileCount;
    public int BaseProjectileHP;

    public float BaseProjectileSpeed;
    public float FireRate;

    public float BaseDMG;
    public float DMG_Multiply;
}

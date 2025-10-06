using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data")]
public class PlayerDataSO : ScriptableObject
{

    public float MaxHealth;
    public float BaseMovementSpeed;
    public float AttackSpeed;
    public float MS_Multiply;
    public float XP_Multiply;
    public float PickUpRange;
}

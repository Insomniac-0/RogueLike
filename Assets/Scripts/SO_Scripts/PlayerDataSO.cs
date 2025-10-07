using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data")]
public class PlayerDataSO : ScriptableObject
{

    public float MaxHealth;
    public float MovementSpeed;
    public float AttackSpeed;
    public float PickUpRange;
}

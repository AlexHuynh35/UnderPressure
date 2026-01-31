using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    [Header("Weapon Stats")]
    public AbilityGroup basicAttack;
    public AbilityGroup specialAttack;
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Consumable", menuName = "Abilities/Justine/Consumable")]
public class Consumable : Ability
{
    public override void OnPress(EntityManager caster, Vector3 aimLocation)
    {
        if (caster.inventory is PlayerInventory inventory)
        {
            if (inventory.UsePotion())
            {
                if (inventory.potion.item is Potion potion)
                {
                    potion.ApplyEffects(caster);
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Consumable", menuName = "Abilities/Justine/Consumable")]
public class Consumable : Ability
{
    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        if (caster.inventory.potion.amount > 0)
        {
            caster.inventory.potion.amount--;

            if (caster.inventory.potion.item is Potion potion)
            {
                potion.ApplyEffects(caster);
            }
        }
    }
}

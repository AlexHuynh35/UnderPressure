using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NormalArrow", menuName = "Item/Arrow/NormalArrow")]
public class NormalArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public bool piercing;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
        };

        return effects;
    }
}

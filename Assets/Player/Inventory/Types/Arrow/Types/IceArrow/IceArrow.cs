using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IceArrow", menuName = "Item/Arrow/IceArrow")]
public class IceArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public bool piercing;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new MovementStatusEffect(boost: 0.75f, rate: 0.5f, duration: 2f, source: caster, allowedTags: EntityTag.Enemy),
        };

        return effects;
    }
}

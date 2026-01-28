using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FireArrow", menuName = "Item/Arrow/FireArrow")]
public class FireArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public bool piercing;
    public float rate;
    public float duration;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new BurnStatusEffect(damage: damage, rate: rate, duration: duration, source: caster, allowedTags: EntityTag.Enemy),
        };

        return effects;
    }
}

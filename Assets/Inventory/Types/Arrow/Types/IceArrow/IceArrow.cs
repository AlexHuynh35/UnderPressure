using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IceArrow", menuName = "Item/Arrow/IceArrow")]
public class IceArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public float boost;
    public bool piercing;
    public float length;
    public float duration;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new FrostStatusEffect(boost: boost, freezeLength: length, rate: duration, duration: duration, source: caster, allowedTags: EntityTag.Enemy),
        };

        return effects;
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DrainArrow", menuName = "Item/Arrow/DrainArrow")]
public class DrainArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public float heal;
    public float lifetime;
    public float rate;
    public float duration;
    public bool piercing;
    public GameObject hitboxPrefab;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new DrainStatusEffect(damage: damage, heal: heal, lifetime: lifetime, hitboxPrefab: hitboxPrefab, rate: rate, duration: duration, source: caster, allowedTags: EntityTag.Enemy)
        };

        return effects;
    }
}

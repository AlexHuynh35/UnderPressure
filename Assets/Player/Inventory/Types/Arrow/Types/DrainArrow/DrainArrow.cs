using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

[CreateAssetMenu(fileName = "DrainArrow", menuName = "Item/Arrow/DrainArrow")]
public class DrainArrow : Arrow
{
    [Header("Arrow Stats")]
    public float damage;
    public bool piercing;
    public GameObject hitboxPrefab;

    public override List<Effect> ReturnEffects(EntityManager caster)
    {
        SpawnData data = new SpawnData
        (
            effects: new List<Effect>() { new HealEffect(heal: 2f, source: caster, allowedTags: EntityTag.Player) },
            hitboxPrefab: hitboxPrefab,
            shape: new CircleShape(radius: 0.25f),
            movement: new StationaryMovement(),
            lifetime: 5f,
            targetSelf: true,
            destroyOnHit: true
        );

        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new SpawnOnHitEffect(data: data, source: caster, allowedTags: EntityTag.Enemy)
        };

        return effects;
    }
}

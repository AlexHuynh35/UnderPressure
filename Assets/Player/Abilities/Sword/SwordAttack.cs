using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SwordAttack", menuName = "Abilities/Justine/SwordAttack", order = 1)]
public class SwordAttack : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public float knockback;

    [Header("Hitboxes")]
    public float radius;
    public float angle;
    public GameObject hitboxPrefab;

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        Effect movementEffect = new MovementEffect(magnitude: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);

        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(magnitude: damage, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new KnockbackEffect(magnitude: knockback, source: caster, allowedTags: EntityTag.Soldier),

        };
        HitboxShape shape = new ConeShape(radius: radius, angle: angle, direction: caster.orientation);
        HitboxMovement movement = new FollowMovement(following: caster, offset: Vector2.zero);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: 0.25f,
            targetSelf: false,
            destroyOnHit: false
        );
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(magnitude: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

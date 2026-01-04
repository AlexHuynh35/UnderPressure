using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightSwordAttack", menuName = "Abilities/Justine/Sword/KnightSword/KnightSwordAttack", order = 1)]
public class KnightSwordAttack : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float knockback;
    public float charge;

    [Header("Hitboxes")]
    public float radius;
    public float angle;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new KnockbackEffect(force: knockback, source: caster, allowedTags: EntityTag.Soldier),
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

        Effect chargeEffect = new ChargeEffect(force: charge, direction: caster.orientation, source: caster, allowedTags: EntityTag.Player);
        chargeEffect.OnEnter(caster);
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

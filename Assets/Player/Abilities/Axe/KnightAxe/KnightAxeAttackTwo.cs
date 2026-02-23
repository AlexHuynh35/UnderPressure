using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightAxeAttackTwo", menuName = "Abilities/Justine/Axe/KnightAxe/KnightAxeAttackTwo")]
public class KnightAxeAttackTwo : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float stunLength;
    public float internalizedLength;

    [Header("Hitboxes")]
    public float radius;
    public float height;
    public float angle;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new StunStatusEffect(rate: 1f, duration: stunLength, source: caster, allowedTags: EntityTag.Soldier),
        };
        HitboxShape shape = new ConeShape(radius: radius, height: height, angle: angle, direction: caster.orientation);
        HitboxMovement movement = new FollowMovement(following: caster, offset: Vector3.zero);
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

        Effect internalizedEffect = new InternalizedStatusEffect(rate: internalizedLength, duration: internalizedLength, source: caster, allowedTags: EntityTag.Player);
        internalizedEffect.OnEnter(caster);
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

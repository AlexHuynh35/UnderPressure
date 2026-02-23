using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ArcherMovesetTwoAttack", menuName = "Abilities/Enemies/Knight/Archer/ArcherMovesetTwoAttack")]
public class ArcherMovesetTwoAttack: Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float angle;

    [Header("Hitboxes")]
    public float radius;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }

    public override void WhileActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Player),
        };
        HitboxShape shape = new SphereShape(radius: radius);
        HitboxMovement movement = new StraightMovement(speed: speed, direction: AbilityHelper.AddOffset(caster.orientation, angle, Random.Range(-2, 3)));
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: lifetime,
            targetSelf: false,
            destroyOnHit: true
        );
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }
}

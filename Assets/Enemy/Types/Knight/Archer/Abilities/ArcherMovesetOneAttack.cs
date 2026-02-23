using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ArcherMovesetOneAttack", menuName = "Abilities/Enemies/Knight/Archer/ArcherMovesetOneAttack")]
public class ArcherMovesetOneAttack: Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

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

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Player),
        };
        HitboxShape shape = new SphereShape(radius: radius);
        HitboxMovement movement = new StraightMovement(speed: speed, direction: caster.orientation);
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

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightAxeAttackSwitchOne", menuName = "Abilities/Justine/Axe/KnightAxe/KnightAxeAttackSwitchOne")]
public class KnightAxeAttackSwitchOne : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float radius;
    public float height;
    public float rate;
    public float speed;
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
            new DamageAreaEffect(damage: damage, piercing: piercing, rate: rate, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
        };
        HitboxShape shape = new DiskShape(radius: radius, height: height);
        HitboxMovement movement = new BoomerangMovement(speed: speed, time: activeTime, direction: caster.orientation);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(caster.orientation.z, caster.orientation.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: activeTime,
            targetSelf: false,
            destroyOnHit: false
        );
    }
    
    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightBowAttackFinal", menuName = "Abilities/Justine/Bow/KnightBow/KnightBowAttackFinal")]
public class KnightBowAttackFinal: Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float knockback;

    [Header("Hitboxes")]
    public float radius;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage * Mathf.Max(chargeTime, 1), piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
        };
        HitboxShape shape = new SphereShape(radius: radius);
        HitboxMovement movement = new StraightMovement(speed: speed, direction: caster.orientation);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(caster.orientation.z, caster.orientation.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: lifetime,
            targetSelf: false,
            destroyOnHit: false
        );

        Effect chargeEffect = new ChargeEffect(force: knockback, direction: -caster.orientation, source: caster, allowedTags: EntityTag.Player);
        chargeEffect.OnEnter(caster);
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

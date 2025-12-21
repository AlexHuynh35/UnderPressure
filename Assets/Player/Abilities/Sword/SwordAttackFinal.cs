using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SwordAttackFinal", menuName = "Abilities/Justine/SwordAttackFinal", order = 2)]
public class SwordAttackFinal : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float knockback;
    public float charge;

    [Header("Hitboxes")]
    public float width;
    public float height;
    public float speed;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage * chargeTime, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            new KnockbackEffect(force: knockback, source: caster, allowedTags: EntityTag.Soldier),
        };
        HitboxShape shape = new BoxShape(x: width, y: height);
        HitboxMovement movement = new StraightMovement(speed: speed, direction: caster.orientation);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
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

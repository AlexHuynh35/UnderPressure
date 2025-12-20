using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AxeAttackFinal", menuName = "Abilities/Justine/AxeAttackFinal", order = 5)]
public class AxeAttackFinal : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float radius;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageAreaEffect(damage: damage, piercing: piercing, rate: 0.25f, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy)
        };
        HitboxShape shape = new CircleShape(radius: radius);
        HitboxMovement movement = new FollowMovement(following: caster, offset: Vector2.zero);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: 0.25f * chargeTime,
            targetSelf: false,
            destroyOnHit: false
        );
    }
}

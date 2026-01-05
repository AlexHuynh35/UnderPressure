using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FlameBladeAttackFinal", menuName = "Abilities/Justine/Sword/FlameBlade/FlameBladeAttackFinal")]
public class FlameBladeAttackFinal : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float radius;
    public float lifetime;
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
        HitboxMovement movement = new StationaryMovement();
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: lifetime * chargeTime,
            targetSelf: false,
            destroyOnHit: false
        );
    }
}

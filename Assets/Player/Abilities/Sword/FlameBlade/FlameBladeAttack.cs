using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FlameBladeAttack", menuName = "Abilities/Justine/Sword/FlameBlade/FlameBladeAttack", order = 1)]
public class FlameBladeAttack : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float rate;
    public float duration;

    [Header("Hitboxes")]
    public float radius;
    public int angle;
    public int number;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        for (int i = -(int)Mathf.Floor(number / 2); i <= (int)Mathf.Floor(number / 2); i++)
        {
            List<Effect> effects = new List<Effect>()
            {
                new DamageStatusEffect(damage: damage, piercing: piercing, rate: rate, duration: duration, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            };
            HitboxShape shape = new CircleShape(radius: radius);
            Vector2 directionOffset = AbilityHelper.AddOffset(caster.orientation, angle / (number + 1), i);
            HitboxMovement movement = new AccelerateMovement(speed: speed, acceleration: -speed, direction: directionOffset);
            HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
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
        }
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

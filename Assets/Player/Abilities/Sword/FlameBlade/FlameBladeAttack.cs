using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FlameBladeAttack", menuName = "Abilities/Justine/Sword/FlameBlade/FlameBladeAttack")]
public class FlameBladeAttack : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float rate;
    public float duration;

    [Header("Hitboxes")]
    public float radius;
    public float height;
    public int angle;
    public int number;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects1 = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
        };
        HitboxShape shape1 = new ConeShape(radius: radius, height: height, angle: angle, direction: caster.orientation);
        HitboxMovement movement1 = new FollowMovement(following: caster, offset: Vector3.zero);
        HitboxManager attack1 = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
        attack1.Initialize
        (
            owner: caster.gameObject,
            effects: effects1,
            shape: shape1,
            movement: movement1,
            lifetime: 0.25f,
            targetSelf: false,
            destroyOnHit: false
        );

        for (int i = -(int)Mathf.Floor(number / 2); i <= (int)Mathf.Floor(number / 2); i++)
        {
            List<Effect> effects2 = new List<Effect>()
            {
                new BurnStatusEffect(damage: damage, rate: rate, duration: duration, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            };
            HitboxShape shape2 = new DiskShape(radius: radius, height: height);
            Vector3 directionOffset = AbilityHelper.AddOffset(caster.orientation, angle / (number + 1), i);
            HitboxMovement movement2 = new AccelerateMovement(speed: speed, acceleration: -speed, direction: directionOffset);
            HitboxManager attack2 = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(directionOffset.z, directionOffset.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
            attack2.Initialize
            (
                owner: caster.gameObject,
                effects: effects2,
                shape: shape2,
                movement: movement2,
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

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
    public float height;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageAreaEffect(damage: damage, piercing: piercing, rate: 0.25f, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy)
        };
        HitboxShape shape = new DiskShape(radius: radius, height: height);
        HitboxMovement movement = new StationaryMovement();
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(caster.orientation.z, caster.orientation.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: lifetime * Mathf.Max(chargeTime, 1),
            targetSelf: false,
            destroyOnHit: false
        );
    }
}

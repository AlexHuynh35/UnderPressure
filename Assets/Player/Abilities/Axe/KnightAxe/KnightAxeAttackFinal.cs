using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightAxeAttackFinal", menuName = "Abilities/Justine/Axe/KnightAxe/KnightAxeAttackFinal")]
public class KnightAxeAttackFinal : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float rate;
    public float movementDebuff;

    [Header("Hitboxes")]
    public float radius;
    public float height;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        caster.ToggleRotate(true);

        Effect movementEffect = new MovementEffect(boost: movementDebuff, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageAreaEffect(damage: damage * Mathf.Max(chargeTime, 1), piercing: piercing, rate: rate, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy)
        };
        HitboxShape shape = new DiskShape(radius: radius, height: height);
        HitboxMovement movement = new FollowMovement(following: caster, offset: Vector2.zero);
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
        caster.ToggleRotate(false);
        
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

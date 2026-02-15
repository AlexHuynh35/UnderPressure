using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ExecutionerMovesetTwoAttack", menuName = "Abilities/Enemies/Knight/Executioner/ExecutionerMovesetTwoAttack")]
public class ExecutionerMovesetTwoAttack : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float rate;
    public float movementBuff;

    [Header("Hitboxes")]
    public float radius;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageAreaEffect(damage: damage * Mathf.Max(chargeTime, 1), piercing: piercing, rate: rate, source: caster, allowedTags: EntityTag.Player)
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
            lifetime: activeTime,
            targetSelf: false,
            destroyOnHit: false
        );

        caster.ToggleRotate(true);

        Effect movementEffect = new MovementEffect(boost: movementBuff, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }

    public override void EndActive(EntityManager caster)
    {
        caster.ToggleRotate(false);
        
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }
}

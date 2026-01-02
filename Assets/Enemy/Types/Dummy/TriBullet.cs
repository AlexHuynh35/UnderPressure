using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TriBullet", menuName = "Abilities/Dummy/TriBullet", order = 2)]
public class TriBullet : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float radius;
    public float angle;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        for (int i = -1; i <= 1; i++)
        {
            List<Effect> effects = new List<Effect>()
            {
                new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Player),
            };
            HitboxShape shape = new CircleShape(radius: radius);
            Vector2 directionOffset = AbilityHelper.AddOffset(AbilityHelper.GetDirection(caster.transform.position, direction), angle, i);
            HitboxMovement movement = new StraightMovement(speed: speed, direction: directionOffset);
            HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
            attack.Initialize
            (
                owner: caster.gameObject,
                effects: effects,
                shape: shape,
                movement: movement,
                lifetime: lifetime,
                targetSelf: false,
                destroyOnHit: true
            );
        }
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Enemy);
        movementEffect.OnEnter(caster);
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightBowAttackOne", menuName = "Abilities/Justine/Bow/KnightBow/KnightBowAttackOne")]
public class KnightBowAttackOne : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float radius;
    public float speed;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0.5f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void WhileHold(EntityManager caster, Vector2 direction)
    {
        if (caster.inventory is PlayerInventory inventory)
        {
            if (inventory.UseArrow())
            {
                List<Effect> effects = new List<Effect>();
                if (inventory.arrow.item is Arrow arrow)
                {
                    effects = arrow.ReturnEffects(caster);
                }
                HitboxShape shape = new CircleShape(radius: radius);
                HitboxMovement movement = new StraightMovement(speed: speed, direction: caster.orientation);
                HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
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
    }

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        if (caster.inventory is PlayerInventory inventory)
        {
            if (inventory.UseArrow())
            {
                List<Effect> effects = new List<Effect>();
                if (inventory.arrow.item is Arrow arrow)
                {
                    effects = arrow.ReturnEffects(caster);
                }
                HitboxShape shape = new CircleShape(radius: radius);
                HitboxMovement movement = new StraightMovement(speed: speed, direction: caster.orientation);
                HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
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
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

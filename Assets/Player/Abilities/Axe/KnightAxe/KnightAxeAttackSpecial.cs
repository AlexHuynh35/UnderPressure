using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightAxeAttackSpecial", menuName = "Abilities/Justine/Axe/KnightAxe/KnightAxeAttackSpecial")]
public class KnightAxeAttackSpecial : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float internalizedLength;

    [Header("Hitboxes")]
    public float width;
    public float height;
    public float lifetime;
    public float speed;
    public bool flipped;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        if (caster.effectManager.CountEffects(typeof(InternalizedStatusEffectInstance)) > 0)
        {
            Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
            movementEffect.OnEnter(caster);
        }
        else
        {
            Effect healEffect = new HealEffect(heal: 0f, source: caster, allowedTags: EntityTag.Player);
            healEffect.OnEnter(caster);
        }
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        if (caster.effectManager.CountEffects(typeof(InternalizedStatusEffectInstance)) > 0)
        {
            List<Effect> effects = new List<Effect>()
            {
                new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
            };
            HitboxShape shape = new BoxShape(x: width, y: height);
            HitboxMovement movement = new AccelerateMovement(speed: speed, acceleration: -speed, direction: caster.orientation);
            Vector2 directionOffset = new Vector2(caster.orientation.x == 0 ? flipped ? 1 : -1 : caster.orientation.x, caster.orientation.y == 0 ? flipped ? 1 : -1 : caster.orientation.y);
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

            Effect internalizedEffect = new InternalizedStatusEffect(rate: internalizedLength, duration: internalizedLength, source: caster, allowedTags: EntityTag.Player);
            internalizedEffect.OnEnter(caster);
        }
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

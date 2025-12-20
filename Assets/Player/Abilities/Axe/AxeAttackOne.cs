using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AxeAttackOne", menuName = "Abilities/Justine/AxeAttackOne", order = 3)]
public class AxeAttackOne : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;

    [Header("Hitboxes")]
    public float width;
    public float height;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector2 direction, float chargeTime)
    {
        List<Effect> effects = new List<Effect>()
        {
            new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy)
        };
        HitboxShape shape = new BoxShape(x: width, y: height);
        HitboxMovement movement = new FollowMovement(following: caster, offset: (Vector3)caster.orientation * 0.5f * width);
        HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position + ((Vector3)caster.orientation * 0.5f * width), Quaternion.Euler(0, 0, Mathf.Atan2(caster.orientation.y, caster.orientation.x) * Mathf.Rad2Deg)).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: caster.gameObject,
            effects: effects,
            shape: shape,
            movement: movement,
            lifetime: 0.25f,
            targetSelf: false,
            destroyOnHit: false
        );
    }
    
    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

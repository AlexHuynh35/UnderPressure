using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightBowAttackTwo", menuName = "Abilities/Justine/Bow/KnightBow/KnightBowAttackTwo")]
public class KnightBowAttackTwo : Ability
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

    public override void OnRelease(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        if (caster.inventory is PlayerInventory inventory)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (inventory.UseArrow())
                {
                    List<Effect> effects = new List<Effect>();
                    if (inventory.arrow.item is Arrow arrow)
                    {
                        effects = arrow.ReturnEffects(caster);
                    }
                    HitboxShape shape = new SphereShape(radius: radius);
                    Vector3 directionOffset = AbilityHelper.AddOffset(caster.orientation, angle, i);
                    HitboxMovement movement = new StraightMovement(speed: speed, direction: directionOffset);
                    HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(directionOffset.z, directionOffset.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
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
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

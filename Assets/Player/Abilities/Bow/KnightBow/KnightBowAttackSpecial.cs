using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightBowAttackSpecial", menuName = "Abilities/Justine/Bow/KnightBow/KnightBowAttackSpecial")]
public class KnightBowAttackSpecial : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public int number;

    [Header("Hitboxes")]
    public float radius;
    public float width;
    public float height;
    public float lifetime;
    public GameObject hitboxPrefab;

    public override void OnPress(EntityManager caster, Vector3 aimLocation)
    {
        Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        if (caster.inventory is PlayerInventory inventory)
        {
            for (int i = 0; i < number; i++)
            {
                if (inventory.UseArrow())
                {
                    List<Effect> arrowEffects = new List<Effect>();
                    if (inventory.arrow.item is Arrow arrow)
                    {
                        arrowEffects = arrow.ReturnEffects(caster);
                    }
                    SpawnData data = new SpawnData
                    (
                        effects: arrowEffects,
                        hitboxPrefab: hitboxPrefab,
                        shape: new SphereShape(radius: radius),
                        movement: new StationaryMovement(),
                        lifetime: 0.25f,
                        targetSelf: false,
                        destroyOnHit: true
                    );

                    List<Effect> effects = new List<Effect>()
                    {
                        new SpawnOnExpireEffect(data: data, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
                    };
                    HitboxShape shape = new SphereShape(radius: 0.25f);
                    Vector2 locationOffset = AbilityHelper.OffsetLocation(aimLocation, width);
                    HitboxMovement movement = new ArchMovement(speed: Vector2.Distance(caster.transform.position, locationOffset) / lifetime, height: height, startLocation: caster.transform.position, endLocation: locationOffset);
                    HitboxManager attack = Instantiate(hitboxPrefab, caster.transform.position, Quaternion.Euler(0, Mathf.Atan2(caster.orientation.z, caster.orientation.x) * Mathf.Rad2Deg, 0)).GetComponent<HitboxManager>();
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
        }
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

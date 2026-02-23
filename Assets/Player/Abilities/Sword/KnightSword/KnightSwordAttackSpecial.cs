using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KnightSwordAttackSpecial", menuName = "Abilities/Justine/Sword/KnightSword/KnightSwordAttackSpecial")]
public class KnightSwordAttackSpecial : Ability
{
    [Header("Ability Stats")]
    public float damage;
    public bool piercing;
    public float charge;

    [Header("Hitboxes")]
    public float width;
    public float height;
    public float length;
    public GameObject hitboxPrefab;

    public override void StartActive(EntityManager caster, Vector3 aimLocation, float chargeTime)
    {
        if (caster is PlayerEntityManager player)
        {
            if (player.shieldManager.UseParry())
            {
                Effect movementEffect = new MovementEffect(boost: 0f, source: caster, allowedTags: EntityTag.Player);
                movementEffect.OnEnter(caster);

                Effect invincibilityEffect = new InvincibilityStatusEffect(frame: false, rate: 1f, duration: activeTime, source: caster, allowedTags: EntityTag.Player);
                invincibilityEffect.OnEnter(caster);

                List<Effect> effects = new List<Effect>()
                {
                    new DamageEffect(damage: damage, piercing: piercing, source: caster, allowedTags: EntityTag.Breakable | EntityTag.Enemy),
                };
                HitboxShape shape = new BoxShape(x: width, y: height, z: length);
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

                Effect chargeEffect = new ChargeEffect(force: charge, direction: caster.orientation, source: caster, allowedTags: EntityTag.Player);
                chargeEffect.OnEnter(caster);
            }
        }
    }

    public override void EndActive(EntityManager caster)
    {
        Effect movementEffect = new MovementEffect(boost: 1f, source: caster, allowedTags: EntityTag.Player);
        movementEffect.OnEnter(caster);
    }
}

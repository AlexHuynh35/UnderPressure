using UnityEngine;
using System.Collections.Generic;

public class DecayStatusEffect : StatusEffect
{
    public float damage;

    public DecayStatusEffect(float damage, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.damage = damage;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new DecayStatusEffectInstance(damage, rate, duration, source);
    }
}

public class DecayStatusEffectInstance : StatusEffectInstance
{
    public float damage;

    public DecayStatusEffectInstance(float damage, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.damage = damage;
    }

    public override void Apply(EntityManager target)
    {
        target.ToggleDecay(true);
        target.spriteManager.ApplyColors(StatusTag.Poison);
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyDamage(damage, false, source.attackMultiplier);
    }

    public override void Revert(EntityManager target)
    {
        target.ToggleDecay(false);
        target.spriteManager.RemoveColors(StatusTag.Poison);
    }
}

public class DrainStatusEffect : StatusEffect
{
    public float damage;
    public SpawnData data;

    public DrainStatusEffect(float damage, float heal, float lifetime, GameObject hitboxPrefab, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.damage = damage;

        data = new SpawnData
        (
            effects: new List<Effect>() { new HealEffect(heal: heal, source: source, allowedTags: EntityTag.Player) },
            hitboxPrefab: hitboxPrefab,
            shape: new SphereShape(radius: 0.25f),
            movement: new StationaryMovement(),
            lifetime: lifetime,
            targetSelf: true,
            destroyOnHit: true
        );
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new DrainStatusEffectInstance(damage, data, rate, duration, source);
    }
}

public class DrainStatusEffectInstance : StatusEffectInstance
{
    public float damage;
    public SpawnData data;

    public DrainStatusEffectInstance(float damage, SpawnData data, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.damage = damage;
        this.data = data;
    }

    public override void Apply(EntityManager target)
    {
        target.spriteManager.ApplyColors(StatusTag.Poison);
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyDamage(damage, false, source.attackMultiplier);
        HitboxManager attack = Object.Instantiate(data.hitboxPrefab, AbilityHelper.OffsetLocation(target.transform.position, 1f), Quaternion.identity).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: source.gameObject,
            effects: data.effects,
            shape: data.shape,
            movement: data.movement,
            lifetime: data.lifetime,
            targetSelf: data.targetSelf,
            destroyOnHit: data.destroyOnHit
        );
    }

    public override void Revert(EntityManager target)
    {
        target.spriteManager.RemoveColors(StatusTag.Poison);
    }
}

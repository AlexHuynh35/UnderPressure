using UnityEngine;

public class BurnStatusEffect : StatusEffect
{
    public float damage;

    public BurnStatusEffect(float damage, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.damage = damage;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new BurnStatusEffectInstance(damage, rate, duration, source);
    }
}

public class BurnStatusEffectInstance : StatusEffectInstance
{
    public float damage;

    public BurnStatusEffectInstance(float damage, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.damage = damage;
    }

    public override void Apply(EntityManager target)
    {
        target.spriteManager.ApplyColors(StatusTag.Burn);
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyDamage(damage, true, source.attackMultiplier);
    }

    public override void Revert(EntityManager target)
    {
        target.spriteManager.ApplyColors(StatusTag.None);
    }
}

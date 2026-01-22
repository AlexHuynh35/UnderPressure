using UnityEngine;

public class DamageEffect : Effect
{
    public float damage;
    public bool piercing;
    
    public DamageEffect(float damage, bool piercing, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyDamage(damage, piercing, source.attackMultiplier);
    }
}

public class DamageAreaEffect : AreaEffect
{
    public float damage;
    public bool piercing;

    public DamageAreaEffect(float damage, bool piercing, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new DamageAreaEffectInstance(damage, piercing, rate, source);
    }
}

public class DamageAreaEffectInstance : AreaEffectInstance
{
    public float damage;
    public bool piercing;

    public DamageAreaEffectInstance(float damage, bool piercing, float rate, EntityManager source) : base(rate, source)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyDamage(damage, piercing, source.attackMultiplier);
    }
}

public class DamageStatusEffect : StatusEffect
{
    public float damage;
    public bool piercing;

    public DamageStatusEffect(float damage, bool piercing, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new DamageStatusEffectInstance(damage, piercing, rate, duration, source);
    }
}

public class DamageStatusEffectInstance : StatusEffectInstance
{
    public float damage;
    public bool piercing;

    public DamageStatusEffectInstance(float damage, bool piercing, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyDamage(damage, piercing, source.attackMultiplier);
    }
}

using UnityEngine;

public class DamageEffect : Effect
{
    public float magnitude;
    
    public DamageEffect(float magnitude, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyDamage(magnitude);
    }
}

public class DamageAreaEffect : AreaEffect
{
    public float magnitude;

    public DamageAreaEffect(float magnitude, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new DamageAreaEffectInstance(magnitude, rate, source);
    }
}

public class DamageAreaEffectInstance : AreaEffectInstance
{
    public float magnitude;

    public DamageAreaEffectInstance(float magnitude, float rate, EntityManager source) : base(rate, source)
    {
        this.magnitude = magnitude;
    }

    public override void Apply(EntityManager target)
    {
        target.ApplyDamage(magnitude);
    }
}

public class DamageStatusEffect : StatusEffect
{
    public float magnitude;

    public DamageStatusEffect(float magnitude, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new DamageStatusEffectInstance(magnitude, rate, duration, source);
    }
}

public class DamageStatusEffectInstance : StatusEffectInstance
{
    public float magnitude;

    public DamageStatusEffectInstance(float magnitude, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.magnitude = magnitude;
    }

    public override void Apply(EntityManager target)
    {
        target.ApplyDamage(magnitude);
    }
}

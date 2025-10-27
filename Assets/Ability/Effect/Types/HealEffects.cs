using UnityEngine;

public class HealEffect : Effect
{
    public float magnitude;

    public HealEffect(float magnitude, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyHeal(magnitude);
    }
}

public class HealAreaEffect : AreaEffect
{
    public float magnitude;

    public HealAreaEffect(float magnitude, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new HealAreaEffectInstance(magnitude, rate, source);
    }
}

public class HealAreaEffectInstance : AreaEffectInstance
{
    public float magnitude;

    public HealAreaEffectInstance(float magnitude, float rate, EntityManager source) : base(rate, source)
    {
        this.magnitude = magnitude;
    }

    public override void Apply(EntityManager target)
    {
        target.ApplyHeal(magnitude);
    }
}

public class HealStatusEffect : StatusEffect
{
    public float magnitude;
    
    public HealStatusEffect(float magnitude, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new HealStatusEffectInstance(magnitude, rate, duration, source);
    }
}

public class HealStatusEffectInstance : StatusEffectInstance
{
    public float magnitude;

    public HealStatusEffectInstance(float magnitude, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.magnitude = magnitude;
    }

    public override void Apply(EntityManager target)
    {
        target.ApplyHeal(magnitude);
    }
}

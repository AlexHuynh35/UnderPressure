using UnityEngine;

public class HealEffect : Effect
{
    public float heal;

    public HealEffect(float heal, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.heal = heal;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyHeal(heal);
    }
}

public class HealAreaEffect : AreaEffect
{
    public float heal;

    public HealAreaEffect(float heal, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.heal = heal;
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new HealAreaEffectInstance(heal, rate, source);
    }
}

public class HealAreaEffectInstance : AreaEffectInstance
{
    public float heal;

    public HealAreaEffectInstance(float heal, float rate, EntityManager source) : base(rate, source)
    {
        this.heal = heal;
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyHeal(heal);
    }
}

public class HealStatusEffect : StatusEffect
{
    public float heal;
    
    public HealStatusEffect(float heal, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.heal = heal;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new HealStatusEffectInstance(heal, rate, duration, source);
    }
}

public class HealStatusEffectInstance : StatusEffectInstance
{
    public float heal;

    public HealStatusEffectInstance(float heal, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.heal = heal;
    }

    public override void Tick(EntityManager target)
    {
        target.ApplyHeal(heal);
    }
}

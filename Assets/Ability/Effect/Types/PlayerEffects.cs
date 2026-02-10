using UnityEngine;
using System;

public class ParryStatusEffect : StatusEffect
{
    public ParryStatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags) { }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new ParryStatusEffectInstance(rate, duration, source);
    }
}

public class ParryStatusEffectInstance : StatusEffectInstance
{
    public ParryStatusEffectInstance(float rate, float duration, EntityManager source) : base(rate, duration, source) { }

    public override void Apply(EntityManager target)
    {
        if (target is PlayerEntityManager player) player.shieldManager.ToggleParry(true);
    }

    public override void Revert(EntityManager target)
    {
        if (target is PlayerEntityManager player) player.shieldManager.ToggleParry(false);
    }
}

public class InternalizedStatusEffect : StatusEffect
{
    public InternalizedStatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags) { }

    public override void OnEnter(EntityManager target)
    {
        StatusEffectInstance ticking = CreateTickingEffect();

        Type type = ticking.GetType();
        if (!target.effectManager.HasEffectList(type))
        {
            target.effectManager.AddEffectList(type, new EffectInstanceList());
        }

        target.effectManager.AddEffect(ticking);
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new InternalizedStatusEffectInstance(rate, duration, source);
    }
}

public class InternalizedStatusEffectInstance : StatusEffectInstance
{
    public InternalizedStatusEffectInstance(float rate, float duration, EntityManager source) : base(rate, duration, source) { }

    public override void Apply(EntityManager target)
    {
        if (target is PlayerEntityManager player) player.ToggleInternalized(true);
    }

    public override void Revert(EntityManager target)
    {
        if (target is PlayerEntityManager player) player.ToggleInternalized(false);
    }
}

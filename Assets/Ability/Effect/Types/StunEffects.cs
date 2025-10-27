using UnityEngine;

public class StunStatusEffect : StatusEffect
{
    public StunStatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags) { }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ToggleStun(true);
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new StunStatusEffectInstance(rate, duration, source);
    }
}

public class StunStatusEffectInstance : StatusEffectInstance
{
    public StunStatusEffectInstance(float rate, float duration, EntityManager source) : base(rate, duration, source) { }
    
    public override void Revert(EntityManager target)
    {
        target.ToggleStun(false);
    }
}

public class SilenceStatusEffect : StatusEffect
{
    public SilenceStatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags) { }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ToggleSilence(true);
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new SilenceStatusEffectInstance(rate, duration, source);
    }
}

public class SilenceStatusEffectInstance : StatusEffectInstance
{
    public SilenceStatusEffectInstance(float rate, float duration, EntityManager source) : base(rate, duration, source) { }

    public override void Revert(EntityManager target)
    {
        target.ToggleSilence(false);
    }
}

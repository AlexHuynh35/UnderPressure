using UnityEngine;

public class ConfuseStatusEffect : StatusEffect
{
    public ConfuseStatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new ConfuseStatusEffectInstance(rate, duration, source);
    }
}

public class ConfuseStatusEffectInstance : StatusEffectInstance
{
    public ConfuseStatusEffectInstance(float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        
    }

    public override void Apply(EntityManager target)
    {
        target.ToggleConfuse(true);
        target.ToggleSilence(true);
        target.spriteManager.ApplyColors(StatusTag.Confuse);
    }

    public override void Revert(EntityManager target)
    {
        target.ToggleConfuse(false);
        target.ToggleSilence(false);
        target.spriteManager.RemoveColors(StatusTag.Confuse);
    }
}

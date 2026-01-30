using UnityEngine;

public class InvincibilityStatusEffect : StatusEffect
{
    public bool frame;

    public InvincibilityStatusEffect(bool frame, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.frame = frame;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new InvincibilityStatusEffectInstance(frame, rate, duration, source);
    }
}

public class InvincibilityStatusEffectInstance : StatusEffectInstance
{
    public bool frame;

    public InvincibilityStatusEffectInstance(bool frame, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.frame = frame;
    }

    public override void Apply(EntityManager target)
    {
        target.ToggleInvincibility(true);
    }

    public override void Tick(EntityManager target)
    {
        if (frame) target.spriteManager.Flash(false);
    }

    public override void Revert(EntityManager target)
    {
        target.ToggleInvincibility(false);
        target.spriteManager.Flash(true);
    }
}

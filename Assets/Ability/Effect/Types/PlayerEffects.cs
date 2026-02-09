using UnityEngine;

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

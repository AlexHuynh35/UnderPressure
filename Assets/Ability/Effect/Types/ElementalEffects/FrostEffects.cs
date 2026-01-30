using UnityEngine;
using System;

public class FrostStatusEffect : StatusEffect
{
    public float boost;
    public float freezeLength;

    public FrostStatusEffect(float boost, float freezeLength, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.boost = boost;
        this.freezeLength = freezeLength;
    }

    public override void OnEnter(EntityManager target)
    {
        StatusEffectInstance ticking = CreateTickingEffect();

        Type type = ticking.GetType();
        if (!target.effectManager.HasEffectList(type))
        {
            target.effectManager.AddEffectList(type, new EffectInstanceList());
        }

        if (target.effectManager.CountEffects(type) < 4)
        {
            ticking.Apply(target);
            target.effectManager.AddEffect(ticking);
        }
        else
        {
            target.effectManager.EmptyEffectList(type, 0);

            Effect effect = new StunStatusEffect(rate: rate, duration: freezeLength, source: source, allowedTags: EntityTag.Enemy | EntityTag.Player);
            effect.OnEnter(target);
        }
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new FrostStatusEffectInstance(boost, rate, duration, source);
    }
}

public class FrostStatusEffectInstance : StatusEffectInstance
{
    public float boost;

    public FrostStatusEffectInstance(float boost, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.boost = boost;
    }

    public override void Apply(EntityManager target)
    {
        target.ChangeMovement(boost, 2);
        target.spriteManager.ApplyColors(StatusTag.Frost);
    }

    public override void Revert(EntityManager target)
    {
        target.ChangeMovement(1, 2);
        target.spriteManager.RemoveColors(StatusTag.Frost);
    }
}

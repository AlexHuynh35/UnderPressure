using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Effect
{
    public EntityManager source;
    public EntityTag allowedTags;

    protected Effect(EntityManager source, EntityTag allowedTags)
    {
        this.source = source;
        this.allowedTags = allowedTags;
    }

    public bool CanApplyTo(EntityManager target)
    {
        return (target.tags & allowedTags) != 0;
    }

    public virtual void OnEnter(EntityManager target) { }
    public virtual void OnLeave(EntityManager target) { }
    public virtual void OnExpire(GameObject hitbox) { }
}

public abstract class AreaEffect : Effect
{
    public float rate;
    private Dictionary<EntityManager, AreaEffectInstance> activeTicks = new();

    protected AreaEffect(float rate, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.rate = rate;
    }

    public override void OnEnter(EntityManager target)
    {
        AreaEffectInstance ticking = CreateTickingEffect();
        activeTicks[target] = ticking;

        Type type = ticking.GetType();
        if (!target.effectManager.HasEffectList(type))
        {
            target.effectManager.AddEffectList(type, new EffectInstanceList());
        }

        if (target.effectManager.CountEffects(type) < 1)
        {
            ticking.Apply(target);
        }

        target.effectManager.AddEffect(ticking);
    }

    public override void OnLeave(EntityManager target)
    {
        if (activeTicks.TryGetValue(target, out AreaEffectInstance ticking))
        {
            ticking.SetExpired();
        }
    }

    public override void OnExpire(GameObject hitbox)
    {
        foreach (var activeTick in activeTicks) {
            activeTick.Value.SetExpired();
        }
    }

    protected abstract AreaEffectInstance CreateTickingEffect();
}

public abstract class StatusEffect : Effect
{
    public float rate;
    public float duration;

    protected StatusEffect(float rate, float duration, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.rate = rate;
        this.duration = duration;
    }

    public override void OnEnter(EntityManager target)
    {
        StatusEffectInstance ticking = CreateTickingEffect();

        Type type = ticking.GetType();
        if (!target.effectManager.HasEffectList(type))
        {
            target.effectManager.AddEffectList(type, new EffectInstanceList());
        }

        if (target.effectManager.CountEffects(type) < 1)
        {
            ticking.Apply(target);
            target.effectManager.AddEffect(ticking);
        }
    }

    protected abstract StatusEffectInstance CreateTickingEffect();
}
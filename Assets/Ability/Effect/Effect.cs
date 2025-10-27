using UnityEngine;
using System.Collections.Generic;

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
    protected int maxStacks = int.MaxValue;
    public virtual int MaxStacks
    {
        get => maxStacks;
        protected set => maxStacks = value;
    }
    protected Dictionary<EntityManager, AreaEffectInstance> activeTicks = new();

    protected AreaEffect(float rate, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.rate = rate;
    }

    public override void OnEnter(EntityManager target)
    {
        AreaEffectInstance ticking = CreateTickingEffect(target);
        activeTicks[target] = ticking;
        if (target.effectManager.AddEffect(ticking, maxStacks)) OnMaxStack(target);
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

    protected virtual void OnMaxStack(EntityManager target) { }

    protected abstract AreaEffectInstance CreateTickingEffect(EntityManager target);
}

public abstract class StatusEffect : Effect
{
    public float rate;
    public float duration;
    public float chance;
    protected int maxStacks = int.MaxValue;
    public virtual int MaxStacks
    {
        get => maxStacks;
        protected set => maxStacks = value;
    }

    protected StatusEffect(float rate, float duration, float chance, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.rate = rate;
        this.duration = duration;
        this.chance = chance;
    }

    public override void OnEnter(EntityManager target)
    {
        if (Random.value < chance)
        {
            StatusEffectInstance ticking = CreateTickingEffect(target);
            if (target.effectManager.AddEffect(ticking, maxStacks)) OnMaxStack(target);
        }
    }

    protected virtual void OnMaxStack(EntityManager target) { }

    protected abstract StatusEffectInstance CreateTickingEffect(EntityManager target);
}

using UnityEngine;

public abstract class EffectInstance
{
    public int maxStacks;
    public EntityManager source;

    protected EffectInstance(int maxStacks, EntityManager source)
    {
        this.maxStacks = maxStacks;
        this.source = source;
    }

    public virtual void OnTick(EntityManager target, float deltaTime, int index) { }

    public virtual void Apply(EntityManager target) { }

    public virtual void Revert(EntityManager target) { }

    public abstract bool IsExpired();
}

public abstract class AreaEffectInstance : EffectInstance
{
    public float rate;
    protected float timeUntilTick;
    protected bool isExpired = false;

    protected AreaEffectInstance(float rate, int maxStacks, EntityManager source) : base(maxStacks, source)
    {
        this.rate = rate;
        timeUntilTick = rate;
    }

    public override void OnTick(EntityManager target, float deltaTime, int index)
    {
        timeUntilTick -= deltaTime;
        if (timeUntilTick <= 0)
        {
            if (index < maxStacks)
            {
                Apply(target);
            }
            timeUntilTick = rate;
        }
    }

    public override bool IsExpired()
    {
        return isExpired;
    }

    public void SetExpired()
    {
        isExpired = true;
    }
}

public abstract class StatusEffectInstance : EffectInstance
{
    public float rate;
    public float duration;
    protected float timeUntilTick;
    protected float elapsedTime = 0;

    protected StatusEffectInstance(float rate, float duration, int maxStacks, EntityManager source) : base(maxStacks, source)
    {
        this.rate = rate;
        this.duration = duration;
        timeUntilTick = rate;
    }

    public override void OnTick(EntityManager target, float deltaTime, int index)
    {
        elapsedTime += deltaTime;
        timeUntilTick -= deltaTime;
        if (timeUntilTick <= 0)
        {
            if (index < maxStacks)
            {
                Apply(target);
            }
            timeUntilTick = rate;
        }
    }

    public override bool IsExpired()
    {
        return duration > 0 && elapsedTime >= duration;
    }
}

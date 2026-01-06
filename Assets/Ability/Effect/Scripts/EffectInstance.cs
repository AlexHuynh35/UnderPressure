using UnityEngine;

public abstract class EffectInstance
{
    public EntityManager source;

    protected EffectInstance(EntityManager source)
    {
        this.source = source;
    }

    public virtual void Apply(EntityManager target) { }

    public virtual void OnTick(EntityManager target, float deltaTime) { }

    public virtual void Tick(EntityManager target) { }

    public virtual void Revert(EntityManager target) { }

    public abstract bool IsExpired();
}

public abstract class AreaEffectInstance : EffectInstance
{
    public float rate;
    protected float timeUntilTick;
    protected bool isExpired = false;

    protected AreaEffectInstance(float rate, EntityManager source) : base(source)
    {
        this.rate = rate;
        timeUntilTick = rate;
    }

    public override void OnTick(EntityManager target, float deltaTime)
    {
        timeUntilTick -= deltaTime;
        if (timeUntilTick <= 0)
        {
            Tick(target);
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

    protected StatusEffectInstance(float rate, float duration, EntityManager source) : base(source)
    {
        this.rate = rate;
        this.duration = duration;
        timeUntilTick = rate;
    }

    public override void OnTick(EntityManager target, float deltaTime)
    {
        elapsedTime += deltaTime;
        timeUntilTick -= deltaTime;
        if (timeUntilTick <= 0)
        {
            Tick(target);
            timeUntilTick = rate;
        }
    }

    public override bool IsExpired()
    {
        return duration > 0 && elapsedTime >= duration;
    }
}
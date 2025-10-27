using UnityEngine;

public class MovementEffect : Effect
{
    public float magnitude;
    
    public MovementEffect(float magnitude, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ChangeMovement(magnitude);
    }
}

public class MovementAreaEffect : AreaEffect
{
    public float magnitude;

    public MovementAreaEffect(float magnitude, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ChangeMovement(magnitude);
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new MovementAreaEffectInstance(magnitude, rate, source);
    }
}

public class MovementAreaEffectInstance : AreaEffectInstance
{
    public float magnitude;

    public MovementAreaEffectInstance(float magnitude, float rate, EntityManager source) : base(rate, source)
    {
        this.magnitude = magnitude;
    }
    
    public override void Revert(EntityManager target)
    {
        target.ChangeMovement(1 / magnitude);
    }
}

public class MovementStatusEffect : StatusEffect
{
    public float magnitude;

    public MovementStatusEffect(float magnitude, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ChangeMovement(magnitude);
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new MovementStatusEffectInstance(magnitude, rate, duration, source);
    }
}

public class MovementStatusEffectInstance : StatusEffectInstance
{
    public float magnitude;

    public MovementStatusEffectInstance(float magnitude, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.magnitude = magnitude;
    }

    public override void Revert(EntityManager target)
    {
        target.ChangeMovement(1f / magnitude);
    }
}

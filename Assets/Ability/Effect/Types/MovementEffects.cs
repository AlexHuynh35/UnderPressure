using UnityEngine;

public class MovementEffect : Effect
{
    public float boost;
    
    public MovementEffect(float boost, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.boost = boost;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ChangeMovement(boost);
    }
}

public class MovementAreaEffect : AreaEffect
{
    public float boost;

    public MovementAreaEffect(float boost, float rate, EntityManager source, EntityTag allowedTags) : base(rate, source, allowedTags)
    {
        this.boost = boost;
    }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ChangeMovement(boost);
    }

    protected override AreaEffectInstance CreateTickingEffect()
    {
        return new MovementAreaEffectInstance(boost, rate, source);
    }
}

public class MovementAreaEffectInstance : AreaEffectInstance
{
    public float boost;

    public MovementAreaEffectInstance(float boost, float rate, EntityManager source) : base(rate, source)
    {
        this.boost = boost;
    }
    
    public override void Revert(EntityManager target)
    {
        target.ChangeMovement(1 / boost);
    }
}

public class MovementStatusEffect : StatusEffect
{
    public float boost;

    public MovementStatusEffect(float boost, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.boost = boost;
    }

    public override void OnEnter(EntityManager target)
    {
        base.OnEnter(target);
        target.ChangeMovement(boost);
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new MovementStatusEffectInstance(boost, rate, duration, source);
    }
}

public class MovementStatusEffectInstance : StatusEffectInstance
{
    public float boost;

    public MovementStatusEffectInstance(float boost, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.boost = boost;
    }

    public override void Revert(EntityManager target)
    {
        target.ChangeMovement(1f / boost);
    }
}

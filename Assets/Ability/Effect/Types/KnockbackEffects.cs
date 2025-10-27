using UnityEngine;

public class KnockbackEffect : Effect
{
    public float magnitude;

    public KnockbackEffect(float magnitude, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.magnitude = magnitude;
    }

    public override void OnEnter(EntityManager target)
    {
        Vector2 direction = (target.transform.position - source.transform.position).normalized;
        target.ApplyKnockback(direction, magnitude);
    }
}

public class ChargeEffect : Effect
{
    public float magnitude;
    public Vector2 direction;

    public ChargeEffect(float magnitude, Vector2 direction, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.magnitude = magnitude;
        this.direction = direction;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyKnockback(direction, magnitude);
    }
}

using UnityEngine;

public class KnockbackEffect : Effect
{
    public float force;

    public KnockbackEffect(float force, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.force = force;
    }

    public override void OnEnter(EntityManager target)
    {
        Vector2 direction = (target.transform.position - source.transform.position).normalized;
        target.ApplyKnockback(direction, force);
    }
}

public class ChargeEffect : Effect
{
    public float force;
    public Vector2 direction;

    public ChargeEffect(float force, Vector2 direction, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.force = force;
        this.direction = direction;
    }

    public override void OnEnter(EntityManager target)
    {
        target.ApplyKnockback(direction, force);
    }
}

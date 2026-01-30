using UnityEngine;

public class BerserkStatusEffect : StatusEffect
{
    public float attackBoost;
    public float damageBoost;

    public BerserkStatusEffect(float attackBoost, float damageBoost, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.attackBoost = attackBoost;
        this.damageBoost = damageBoost;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new BerserkStatusEffectInstance(attackBoost, damageBoost, rate, duration, source);
    }
}

public class BerserkStatusEffectInstance : StatusEffectInstance
{
    public float attackBoost;
    public float damageBoost;

    public BerserkStatusEffectInstance(float attackBoost, float damageBoost, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.attackBoost = attackBoost;
        this.damageBoost = damageBoost;
    }

    public override void Apply(EntityManager target)
    {
        target.ChangeAttack(attackBoost);
        target.ChangeDamage(damageBoost);
        target.spriteManager.ApplyColors(StatusTag.Berserk);
    }

    public override void Revert(EntityManager target)
    {
        target.ChangeAttack(1);
        target.ChangeDamage(1);
        target.spriteManager.RemoveColors(StatusTag.Berserk);
    }
}

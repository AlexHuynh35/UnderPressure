using UnityEngine;

public class HasteStatusEffect : StatusEffect
{
    public float proficiencyBoost;
    public float movementBoost;

    public HasteStatusEffect(float proficiencyBoost, float movementBoost, float rate, float duration, EntityManager source, EntityTag allowedTags) : base(rate, duration, source, allowedTags)
    {
        this.proficiencyBoost = proficiencyBoost;
        this.movementBoost = movementBoost;
    }

    protected override StatusEffectInstance CreateTickingEffect()
    {
        return new HasteStatusEffectInstance(proficiencyBoost, movementBoost, rate, duration, source);
    }
}

public class HasteStatusEffectInstance : StatusEffectInstance
{
    public float proficiencyBoost;
    public float movementBoost;

    public HasteStatusEffectInstance(float proficiencyBoost, float movementBoost, float rate, float duration, EntityManager source) : base(rate, duration, source)
    {
        this.proficiencyBoost = proficiencyBoost;
        this.movementBoost = movementBoost;
    }

    public override void Apply(EntityManager target)
    {
        target.ChangeProficiency(proficiencyBoost);
        target.ChangeMovement(movementBoost, 2);
        target.spriteManager.ApplyColors(StatusTag.Haste);
    }

    public override void Revert(EntityManager target)
    {
        target.ChangeProficiency(1);
        target.ChangeMovement(1, 2);
        target.spriteManager.RemoveColors(StatusTag.Haste);
    }
}

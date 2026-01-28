using UnityEngine;

[CreateAssetMenu(fileName = "BerserkPotion", menuName = "Item/Potion/BerserkPotion")]
public class BerserkPotion : Potion
{
    [Header("Potion Stats")]
    public float attackBoost;
    public float damageBoost;
    public float duration;

    public override void ApplyEffects(EntityManager caster)
    {
        Effect berserkStatusEffect = new BerserkStatusEffect(attackBoost: attackBoost, damageBoost: damageBoost, rate: duration, duration: duration, source: caster, allowedTags: EntityTag.Player);
        berserkStatusEffect.OnEnter(caster);
    }
}

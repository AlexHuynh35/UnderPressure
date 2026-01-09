using UnityEngine;

[CreateAssetMenu(fileName = "RegenerationPotion", menuName = "Item/Potion/RegenerationPotion")]
public class RegenerationPotion : Potion
{
    [Header("Potion Stats")]
    public float heal;
    public float rate;
    public float duration;

    public override void ApplyEffects(EntityManager caster)
    {
        Effect healStatusEffect = new HealStatusEffect(heal: heal, rate: rate, duration: duration, source: caster, allowedTags: EntityTag.Player);
        healStatusEffect.OnEnter(caster);
    }
}

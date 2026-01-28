using UnityEngine;

[CreateAssetMenu(fileName = "HastePotion", menuName = "Item/Potion/HastePotion")]
public class HastePotion : Potion
{
    [Header("Potion Stats")]
    public float proficiencyBoost;
    public float movementBoost;
    public float duration;

    public override void ApplyEffects(EntityManager caster)
    {
        Effect hasteStatusEffect = new HasteStatusEffect(proficiencyBoost: proficiencyBoost, movementBoost: movementBoost, rate: duration, duration: duration, source: caster, allowedTags: EntityTag.Player);
        hasteStatusEffect.OnEnter(caster);
    }
}

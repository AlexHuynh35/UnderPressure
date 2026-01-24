using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Justine/Dash")]
public class Dash : Ability
{
    [Header("Ability Stats")]
    public float charge;

    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        if (caster.UseStamina())
        {
            Effect chargeEffect = new ChargeEffect(force: charge, direction: caster.orientation, source: caster, allowedTags: EntityTag.Player);
            chargeEffect.OnEnter(caster);
        }
    }
}

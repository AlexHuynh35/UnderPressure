using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Shield", menuName = "Abilities/Justine/Shield", order = 10)]
public class Shield : Ability
{
    public override void OnPress(EntityManager caster, Vector2 direction)
    {
        caster.gameObject.GetComponentInChildren<ShieldManager>(true).Activate();
    }

    public override void OnRelease(EntityManager caster, Vector2 direction)
    {
        caster.gameObject.GetComponentInChildren<ShieldManager>(true).Deactivate();
    }
}

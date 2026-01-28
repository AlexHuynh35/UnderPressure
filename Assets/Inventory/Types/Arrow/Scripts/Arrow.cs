using UnityEngine;
using System.Collections.Generic;

public class Arrow : Item
{
    public virtual List<Effect> ReturnEffects(EntityManager caster)
    {
        return new List<Effect>();
    }
}

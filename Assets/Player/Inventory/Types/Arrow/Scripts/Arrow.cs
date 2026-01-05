using UnityEngine;
using System.Collections.Generic;
using System;

public class Arrow : Item
{
    public virtual List<Effect> ReturnEffects(EntityManager caster)
    {
        return new List<Effect>();
    }
}

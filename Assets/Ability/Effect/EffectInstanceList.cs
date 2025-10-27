using UnityEngine;
using System.Collections.Generic;

public class EffectInstanceList
{
    public List<EffectInstance> instances = new();

    public void Add(EffectInstance effect)
    {
        instances.Add(effect);
    }

    public virtual void Update(EntityManager self, float deltaTime)
    {
        for (int i = instances.Count - 1; i >= 0; i--)
        {
            var effect = instances[i];
            effect.OnTick(self, deltaTime, i);
            if (effect.IsExpired())
            {
                effect.Revert(self);
                instances.RemoveAt(i);
            }
        }
    }

    public void ClearInstances()
    {
        instances.Clear();
    }

    public void ClearAllButFirst(int x)
    {
        if (instances.Count > x)
        {
            instances.RemoveRange(x, instances.Count - x);
        }
    }

    public int Count => instances.Count;
    public IEnumerable<EffectInstance> All => instances;
}

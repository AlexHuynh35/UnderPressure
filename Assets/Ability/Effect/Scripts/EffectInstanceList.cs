using UnityEngine;
using System.Collections.Generic;

public class EffectInstanceList
{
    public List<EffectInstance> instances = new();

    public void Add(EffectInstance effect)
    {
        instances.Add(effect);
    }

    public void Update(EntityManager self, float deltaTime)
    {
        if (instances.Count > 0)
        {
            instances[0].OnTick(self, deltaTime);
            if (instances[0].IsExpired())
            {
                instances[0].Revert(self);
                if (instances.Count > 1 && instances[1] != null && !instances[1].IsExpired())
                {
                    instances[1].Apply(self);
                }
                instances.RemoveAt(0);
            }
        }

        for (int i = instances.Count - 1; i >= 0; i--)
        {
            var effect = instances[i];
            if (effect.IsExpired())
            {
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

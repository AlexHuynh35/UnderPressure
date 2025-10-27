using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private List<EffectInstance> activeEffects = new();
    private EntityManager self;

    void Start()
    {
        self = GetComponent<EntityManager>();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.OnTick(self, dt);
            if (effect.IsExpired())
            {
                effect.Revert(self);
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void AddEffect(EffectInstance effect)
    {
        activeEffects.Add(effect);
    }
}
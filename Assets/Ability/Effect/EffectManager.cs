using UnityEngine;
using System.Collections.Generic;
using System;

public class EffectManager : MonoBehaviour
{
    private Dictionary<Type, EffectInstanceList> activeEffects = new();
    private EntityManager self;

    void Start()
    {
        self = GetComponent<EntityManager>();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        foreach (var effectType in activeEffects)
        {
            var effectList = effectType.Value;
            effectList.Update(self, dt);
        }
    }

    public bool HasEffectList(Type type)
    {
        return activeEffects.ContainsKey(type);
    }

    public void emptyEffectList(Type type, int remaining)
    {
        if (activeEffects.TryGetValue(type, out var list))
        {
            if (remaining == 0)
            {
                list.ClearInstances();
            }
            else
            {
                list.ClearAllButFirst(remaining);
            }
        }
    }

    public void AddEffectList(Type type, EffectInstanceList list)
    {
        activeEffects[type] = list;
    }

    public bool AddEffect(EffectInstance effect, int maxStacks)
    {
        Type type = effect.GetType();

        if (activeEffects.TryGetValue(type, out var list))
        {
            list.Add(effect);
        }

        if (list.Count <= maxStacks) return false;
        return true;
    }
}
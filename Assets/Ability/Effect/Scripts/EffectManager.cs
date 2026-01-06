using UnityEngine;
using System;
using System.Collections.Generic;

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

    public void EmptyEffectList(Type type, int remaining)
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

    public void AddEffect(EffectInstance effect)
    {
        Type type = effect.GetType();

        if (activeEffects.TryGetValue(type, out var list))
        {
            list.Add(effect);
        }
    }

    public int CountEffects(Type type)
    {
        if (activeEffects.TryGetValue(type, out var list))
        {
            return list.Count;
        }
        return 0;
    }
}
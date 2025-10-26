using UnityEngine;
using System.Collections.Generic;

public enum AbilityState
{
    Ready,
    Charge,
    Windup,
    Active,
    Cooldown,
    Disabled
}

[System.Serializable]
public struct AbilityTimer
{
    public AbilityState state;
    public float chargeTimer;
    public float windupTimer;
    public float activeTimer;
    public float cooldownTimer;
    public float sustainTimer;
    public float castTimer;
    public Vector2 lockedOrientation;
}

[System.Serializable]
public class AbilitySlot
{
    public KeyCode inputKey;
    public List<Ability> abilities;
    public int mode;
}

public class AbilityManager : MonoBehaviour
{
    public List<AbilitySlot> slots;
    private List<AbilityTimer> timers;
    private EntityManager caster;
    private int numSlots;

    void Start()
    {
        numSlots = slots.Count;
        caster = GetComponent<EntityManager>();
        for (int i = 0; i < numSlots; i++)
        {
            var timer = new AbilityTimer
            {
                state = AbilityState.Ready,
                chargeTimer = 0,
                windupTimer = 0,
                activeTimer = 0,
                cooldownTimer = 0,
                sustainTimer = 0,
                castTimer = 0
            };
            timers[i] = timer;
        }
    }

    void Update()
    {
        UpdateAbilities();
        if (caster.stunned == 0 || caster.silenced == 0)
        {
            InterruptAbilities();
            return;
        }
    }

    protected virtual void StartAbility() { }
    protected virtual void ReleaseAbility() { }
    protected virtual void UpdateAbilities() { }
    protected virtual void InterruptAbilities() { }
}

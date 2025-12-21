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
}

public class AbilityManager : MonoBehaviour
{
    public List<AbilitySlot> slots;
    protected List<AbilityTimer> timers;
    protected EntityManager caster;
    protected int numSlots;
    protected Vector2 aimLocation;
    protected Vector2 lockedAimLocation;

    void Start()
    {
        numSlots = slots.Count;
        caster = GetComponent<EntityManager>();
        timers = new List<AbilityTimer>(slots.Count);
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = new AbilityTimer
            {
                state = AbilityState.Ready,
                chargeTimer = 0,
                windupTimer = 0,
                activeTimer = 0,
                cooldownTimer = 0,
                sustainTimer = 0,
                castTimer = 0
            };
            timers.Add(timer);
        }
    }

    void Update()
    {
        SetAimLocation();
        
        UpdateAbilities();
        if (caster.stunned == 0 || caster.silenced == 0)
        {
            InterruptAbilities();
            return;
        }

        if (!IsActive()) StartAbility();

        ReleaseAbility();
    }

    protected virtual void SetAimLocation() { }
    protected virtual void StartAbility() { }
    protected virtual void ReleaseAbility() { }
    protected virtual void UpdateAbilities() { }
    protected virtual void InterruptAbilities() { }
    protected virtual bool IsActive()
    {
        return false;
    }
}

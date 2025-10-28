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
    protected List<AbilityTimer> timers;
    protected EntityManager caster;
    protected int numSlots;
    protected Vector2 aimLocation;
    protected Vector2 lockedAimLocation;

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
        SetAimLocation();
        
        UpdateAbilities();
        if (caster.stunned == 0 || caster.silenced == 0)
        {
            InterruptAbilities();
            return;
        }

        StartAbility();
        ReleaseAbility();
    }

    protected virtual void SetAimLocation() { }
    protected virtual void StartAbility() { }
    protected virtual void ReleaseAbility() { }
    protected virtual void UpdateAbilities() { }
    protected virtual void InterruptAbilities() { }
}

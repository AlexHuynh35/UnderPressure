using UnityEngine;
using System.Collections.Generic;

public enum AbilityState
{
    Ready,
    Charge,
    WindUp,
    Active,
    Cooldown,
    Disabled
}

[System.Serializable]
public struct AbilityTimer
{
    public AbilityState state;
    public float chargeTimer;
    public float windUpTimer;
    public float activeTimer;
    public float cooldownTimer;
    public float sustainTimer;
    public float castTimer;
}

[System.Serializable]
public class AbilitySlot
{
    public InputType inputType;
    public AbilityGroup group;
}

public class AbilityManager : MonoBehaviour
{
    public List<AbilitySlot> slots;
    protected List<AbilityTimer> timers;
    protected EntityManager caster;
    protected int numSlots;
    protected Vector3 aimLocation;
    protected Vector3 lockedAimLocation;

    private void Awake()
    {
        caster = GetComponent<EntityManager>();
        Initialize();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!InputManager.Instance.receivingInputs) return;
        
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
    
    protected virtual void Initialize()
    {
        numSlots = slots.Count;
        timers = new List<AbilityTimer>(slots.Count);
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = new AbilityTimer
            {
                state = AbilityState.Ready,
                chargeTimer = 0,
                windUpTimer = 0,
                activeTimer = 0,
                cooldownTimer = 0,
                sustainTimer = 0,
                castTimer = 0
            };
            timers.Add(timer);
        }
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

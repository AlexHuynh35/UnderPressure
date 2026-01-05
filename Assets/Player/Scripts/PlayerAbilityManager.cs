using UnityEngine;

public class PlayerAbilityManager : AbilityManager
{
    public int maxModes;
    public float timeBetweenMode;

    private bool canQueueNext;
    private int mode;
    private float timerBetweenMode;

    protected override void SetAimLocation()
    {
        aimLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void StartAbility()
    {
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            AbilitySlot slot = slots[i];

            if (timer.state == AbilityState.Ready && Input.GetKeyDown(slot.inputKey))
            {
                if (canQueueNext)
                {
                    canQueueNext = false;
                    mode++;

                    if (mode >= maxModes) mode = 0;
                }

                timer.state = AbilityState.Charge;
                timer.chargeTimer = 0;
                timer.sustainTimer = slot.group.abilities[mode].sustainSpeed;

                timers[i] = timer;

                slot.group.abilities[mode].OnPress(caster, aimLocation);
            }
        }
    }

    protected override void ReleaseAbility()
    {
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            AbilitySlot slot = slots[i];

            if (timer.state == AbilityState.Charge && Input.GetKeyUp(slot.inputKey))
            {
                timer.state = AbilityState.Windup;
                timer.windupTimer = slot.group.abilities[mode].windupTime;

                timers[i] = timer;
                lockedAimLocation = aimLocation;

                slot.group.abilities[mode].OnRelease(caster, aimLocation);
            }
        }
    }

    protected override void UpdateAbilities()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            Ability slot = slots[i].group.abilities[mode >= slots[i].group.abilities.Count ? 0 : mode];

            switch (timer.state)
            {
                case AbilityState.Charge:
                    timer.chargeTimer += dt;
                    timer.sustainTimer -= dt;
                    if (timer.sustainTimer <= 0)
                    {
                        timer.sustainTimer = slot.sustainSpeed;
                        slot.WhileHold(caster, aimLocation);
                    }
                    break;

                case AbilityState.Windup:
                    timer.windupTimer -= dt;
                    if (timer.windupTimer <= 0)
                    {
                        timer.state = AbilityState.Active;
                        timer.activeTimer = slot.activeTime;
                        timer.castTimer = slot.castSpeed;
                        slot.StartActive(caster, lockedAimLocation, Mathf.Min(timer.chargeTimer, slot.maxChargeTime));
                    }
                    break;

                case AbilityState.Active:
                    timer.activeTimer -= dt;
                    timer.castTimer -= dt;
                    if (timer.castTimer <= 0)
                    {
                        timer.castTimer = slot.castSpeed;
                        slot.WhileActive(caster, lockedAimLocation, Mathf.Min(timer.chargeTimer, slot.maxChargeTime));
                    }
                    if (timer.activeTimer <= 0)
                    {
                        timer.state = AbilityState.Cooldown;
                        timer.cooldownTimer = slots[i].group.cooldownTime;
                        slot.EndActive(caster);
                        if (i < 3)
                        {
                            canQueueNext = true;
                            timerBetweenMode = timeBetweenMode;
                        }
                    }
                    break;

                case AbilityState.Cooldown:
                    timer.cooldownTimer -= dt;
                    if (timer.cooldownTimer <= 0)
                    {
                        timer.state = AbilityState.Ready;
                    }
                    break;
            }

            timers[i] = timer;
        }

        if (canQueueNext)
        {
            timerBetweenMode -= dt;
            if (timerBetweenMode <= 0)
            {
                canQueueNext = false;
                mode = 0;
            }
        }
    }

    protected override void InterruptAbilities()
    {
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            Ability slot = slots[i].group.abilities[mode >= slots[i].group.abilities.Count ? 0 : mode];

            switch (timer.state)
            {
                case AbilityState.Charge:
                    slot.OnRelease(caster, Vector2.zero);
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slots[i].group.cooldownTime;
                    break;

                case AbilityState.Windup:
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slots[i].group.cooldownTime;
                    break;

                case AbilityState.Active:
                    slot.EndActive(caster);
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slots[i].group.cooldownTime;
                    break;
            }

            timers[i] = timer;
        }
    }

    protected override bool IsActive()
    {
        bool isActive = false;
        foreach (AbilityTimer timer in timers)
        {
            isActive = isActive || timer.state == AbilityState.Charge || timer.state == AbilityState.Windup || timer.state == AbilityState.Active;
        }
        return isActive;
    }
}

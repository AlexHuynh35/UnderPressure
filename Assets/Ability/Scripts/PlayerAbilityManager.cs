using UnityEngine;

public class PlayerAbilityManager: AbilityManager
{
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
                timer.state = AbilityState.Charge;
                timer.chargeTimer = 0;
                timer.sustainTimer = slot.abilities[slot.mode].sustainSpeed;

                timers[i] = timer;

                slot.abilities[slot.mode].OnPress(caster, aimLocation);
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
                timer.windupTimer = slot.abilities[slot.mode].windupTime;

                timers[i] = timer;
                lockedAimLocation = aimLocation;

                slot.abilities[slot.mode].OnRelease(caster, aimLocation);
            }
        }
    }

    protected override void UpdateAbilities()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            Ability slot = slots[i].abilities[slots[i].mode];

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
                        timer.cooldownTimer = slot.cooldownTime;
                        slot.EndActive(caster);
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
    }

    protected override void InterruptAbilities()
    {
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            Ability slot = slots[i].abilities[slots[i].mode];

            switch (timer.state)
            {
                case AbilityState.Charge:
                    slot.OnRelease(caster, Vector2.zero);
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slot.cooldownTime;
                    break;

                case AbilityState.Windup:
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slot.cooldownTime;
                    break;

                case AbilityState.Active:
                    slot.EndActive(caster);
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slot.cooldownTime;
                    break;
            }

            timers[i] = timer;
        }
    }
}

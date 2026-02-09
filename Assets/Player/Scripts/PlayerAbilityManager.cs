using UnityEngine;
using System.Collections.Generic;

public class PlayerAbilityManager : AbilityManager
{
    public float timeBetweenMode;

    private bool canQueueNext;
    private int currentAttack;
    private int mode;
    private float timerBetweenMode;

    private int weaponBasicSlot;
    private int weaponSpecialSlot;
    private int numActiveSlots;
    private int shieldSlot;
    private int dashSlot;

    public void SwitchWeapon(Weapon weapon)
    {
        AbilitySlot basicSlot = slots[weaponBasicSlot];
        basicSlot.group = weapon.basicAttack;
        slots[weaponBasicSlot] = basicSlot;

        AbilitySlot specialSlot = slots[weaponSpecialSlot];
        specialSlot.group = weapon.specialAttack;
        slots[weaponSpecialSlot] = specialSlot;
    }

    protected override void Initialize()
    {
        /*
        slots = new List<AbilitySlot>()
        {
            new AbilitySlot { inputType = InputType.WeaponBasic, group = null },
            new AbilitySlot { inputType = InputType.WeaponSpecial, group = null },
            new AbilitySlot { inputType = InputType.ConsumableOne, group = null },
            new AbilitySlot { inputType = InputType.ConsumableTwo, group = null },
            new AbilitySlot { inputType = InputType.Shield, group = null },
            new AbilitySlot { inputType = InputType.Dash, group = null }
        };
        */

        base.Initialize();

        weaponBasicSlot = 0;
        weaponSpecialSlot = 1;
        numActiveSlots = 4;
        shieldSlot = 4;
        dashSlot = 5;
    }

    protected override void SetAimLocation()
    {
        aimLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void StartAbility()
    {
        StartActiveAbilities();
        StartNonActiveAbility(shieldSlot);
        StartNonActiveAbility(dashSlot);
    }

    private void StartActiveAbilities()
    {
        for (int i = 0; i < numActiveSlots; i++)
        {
            AbilityTimer timer = timers[i];
            AbilitySlot slot = slots[i];

            if (timer.state == AbilityState.Ready && InputManager.Instance.inputDict[slot.inputType].WasPressedThisFrame())
            {
                InterruptAbility(shieldSlot);
                InterruptAbility(dashSlot);
                
                if (canQueueNext && currentAttack == i && mode < slot.group.abilities.Count - 1)
                {
                    mode++;
                }
                else
                {
                    mode = 0;
                }

                canQueueNext = false;
                currentAttack = i;

                timer.state = AbilityState.Charge;
                timer.chargeTimer = 0;
                timer.sustainTimer = slot.group.abilities[mode].sustainSpeed;

                timers[i] = timer;

                slot.group.abilities[mode].OnPress(caster, aimLocation);
            }
        }
    }

    private void StartNonActiveAbility(int index)
    {
        AbilityTimer timer = timers[index];
        AbilitySlot slot = slots[index];

        if (timer.state == AbilityState.Ready && InputManager.Instance.inputDict[slot.inputType].WasPressedThisFrame())
        {
            if (index == dashSlot) 
            {
                InterruptAbility(shieldSlot);
            }

            mode = 0;

            timer.state = AbilityState.Charge;
            timer.chargeTimer = 0;
            timer.sustainTimer = slot.group.abilities[mode].sustainSpeed;

            timers[index] = timer;

            slot.group.abilities[mode].OnPress(caster, aimLocation);
        }
    }

    protected override void ReleaseAbility()
    {
        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            AbilitySlot slot = slots[i];

            if (timer.state == AbilityState.Charge && InputManager.Instance.inputDict[slot.inputType].WasReleasedThisFrame())
            {
                timer.state = AbilityState.WindUp;
                timer.windUpTimer = slot.group.abilities[i < numActiveSlots ? mode : 0].windUpTime * caster.proficiency;

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

                case AbilityState.WindUp:
                    timer.windUpTimer -= dt;
                    if (timer.windUpTimer <= 0)
                    {
                        timer.state = AbilityState.Active;
                        timer.activeTimer = slot.activeTime + slot.windDownTime * caster.proficiency;
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
            InterruptAbility(i);
        }
    }

    private void InterruptAbility(int index)
    {
        AbilityTimer timer = timers[index];
        Ability slot = slots[index].group.abilities[mode >= slots[index].group.abilities.Count ? 0 : mode];

        switch (timer.state)
        {
            case AbilityState.Charge:
                slot.OnRelease(caster, Vector2.zero);
                timer.state = AbilityState.Cooldown;
                timer.cooldownTimer = slots[index].group.cooldownTime;
                break;

            case AbilityState.WindUp:
                timer.state = AbilityState.Cooldown;
                timer.cooldownTimer = slots[index].group.cooldownTime;
                break;

            case AbilityState.Active:
                slot.EndActive(caster);
                timer.state = AbilityState.Cooldown;
                timer.cooldownTimer = slots[index].group.cooldownTime;
                break;
        }

        timers[index] = timer;
    }

    protected override bool IsActive()
    {
        bool isActive = false;
        for (int i = 0; i < numActiveSlots; i++)
        {
            isActive = isActive || timers[i].state == AbilityState.Charge || timers[i].state == AbilityState.WindUp || timers[i].state == AbilityState.Active;
        }
        return isActive;
    }

    private bool ShieldActive()
    {
        return timers[shieldSlot].state == AbilityState.Charge || timers[shieldSlot].state == AbilityState.WindUp || timers[shieldSlot].state == AbilityState.Active;
    }

    private bool DashActive()
    {
        return timers[dashSlot].state == AbilityState.Charge || timers[dashSlot].state == AbilityState.WindUp || timers[dashSlot].state == AbilityState.Active;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class EnemyAbilityManager : AbilityManager
{
    public float detectionRange;
    public float timeBetweenAttack;

    private Transform target;
    private float timerBetweenAttack;
    private int currentAttack;
    private int mode;
    private bool continuingCombo;

    protected override void Initialize()
    {
        target = PlayerData.Player?.transform;
    }

    protected override void SetAimLocation()
    {
        if (target != null)
        {
            aimLocation = target.position;
        }
        else
        {
            target = PlayerData.Player.transform;
            aimLocation = target.position;
        }
    }

    protected override void StartAbility()
    {
        if (timerBetweenAttack > 0f && !continuingCombo)
        {
            timerBetweenAttack -= Time.deltaTime;
            return;
        }

        DecideNextAbility();
        ExecuteAbility();
    }

    private void DecideNextAbility()
    {
        if (continuingCombo && currentAttack >= 0)
        {
            int nextMode = mode + 1;

            if (nextMode < slots[currentAttack].group.abilities.Count)
            {
                mode = nextMode;
                continuingCombo = false;
                return;
            }
            else
            {
                mode = 0;
                continuingCombo = false;
                currentAttack = -1;
                return;
            }
        }

        mode = 0;
        continuingCombo = false;
        currentAttack = SelectRandomAbility();

        if (currentAttack >= 0) timerBetweenAttack = timeBetweenAttack;
    }

    private int SelectRandomAbility()
    {
        List<int> usable = new();

        for (int i = 0; i < numSlots; i++)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (timers[i].state == AbilityState.Ready)
            {
                float useRange = slots[i].group.abilities[0].range + target.localScale.x / 2;

                if (dist <= useRange) usable.Add(i);
            }
        }

        if (usable.Count > 0)
        {
            return usable[Random.Range(0, usable.Count)];
        }
        else
        {
            return -1;
        }
    }

    private void ExecuteAbility()
    {
        if (currentAttack >= 0)
        {
            AbilityTimer timer = timers[currentAttack];
            AbilitySlot slot = slots[currentAttack];

            if (timer.state == AbilityState.Ready)
            {
                timer.state = AbilityState.Charge;
                timer.chargeTimer = 0;
                timer.sustainTimer = slot.group.abilities[mode].sustainSpeed;

                timers[currentAttack] = timer;

                slot.group.abilities[mode].OnPress(caster, aimLocation);
            }
        }
    }

    protected override void UpdateAbilities()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            if (mode >= slots[i].group.abilities.Count) break;
            Ability slot = slots[i].group.abilities[mode];

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
                    if (timer.chargeTimer >= slot.maxChargeTime)
                    {
                        timer.state = AbilityState.WindUp;
                        timer.windUpTimer = slot.windUpTime * caster.proficiency;
                        lockedAimLocation = aimLocation;
                        slot.OnRelease(caster, aimLocation);
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
                        if (mode + 1 < slots[i].group.abilities.Count)
                        {
                            timer.state = AbilityState.Ready;
                            continuingCombo = true;
                        }
                        else
                        {
                            timer.state = AbilityState.Cooldown;
                            timer.cooldownTimer = slots[i].group.cooldownTime;
                        }
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
            if (mode >= slots[i].group.abilities.Count) break;
            Ability slot = slots[i].group.abilities[mode];

            switch (timer.state)
            {
                case AbilityState.Charge:
                    slot.OnRelease(caster, Vector2.zero);
                    timer.state = AbilityState.Cooldown;
                    timer.cooldownTimer = slots[i].group.cooldownTime;
                    break;

                case AbilityState.WindUp:
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
            isActive = isActive || timer.state == AbilityState.Charge || timer.state == AbilityState.WindUp || timer.state == AbilityState.Active;
        }
        return isActive;
    }
}

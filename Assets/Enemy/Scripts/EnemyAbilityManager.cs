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
        target = PlayerData.Player.transform;
    }

    protected override void SetAimLocation()
    {
        aimLocation = target.position;
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

            if (nextMode < slots[currentAttack].abilities.Count)
            {
                mode = nextMode;
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
                float useRange = slots[i].abilities[0].range + target.localScale.x / 2;

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
                timer.sustainTimer = slot.abilities[mode].sustainSpeed;

                timers[currentAttack] = timer;

                slot.abilities[mode].OnPress(caster, aimLocation);
            }
        }
    }

    protected override void UpdateAbilities()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < numSlots; i++)
        {
            AbilityTimer timer = timers[i];
            Ability slot = slots[i].abilities[mode];

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
                        timer.state = AbilityState.Windup;
                        timer.windupTimer = slot.windupTime;
                        slot.OnRelease(caster, aimLocation);
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
                        continuingCombo = true;
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
            Ability slot = slots[i].abilities[mode];

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

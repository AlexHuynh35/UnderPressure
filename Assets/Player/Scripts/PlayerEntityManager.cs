using UnityEngine;

public class PlayerEntityManager : EntityManager
{
    [Header("Player Cached Components")]
    public ShieldManager shieldManager;

    [Header("Player Combat Stats")]
    public int stamina;
    public float staminaRate;
    private float staminaTimer;
    public float invincibilityFrames;
    public bool internalized;
    public float internalizedDamage;

    [Header("Player Default Stats")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxStaminaRate;
    [SerializeField] private float maxInternalizedDamageRate;

    void Update()
    {
        RegainStamina();
    }

    public bool UseStamina()
    {
        if (stamina > 0)
        {
            stamina--;
            return true;
        }
        return false;
    }

    public void ChangeStaminaRate(float amount)
    {
        staminaRate = maxStaminaRate * amount;
    }

    private void RegainStamina()
    {
        if (stamina < maxStamina)
        {
            staminaTimer += Time.deltaTime;
            if (staminaTimer >= staminaRate)
            {
                stamina++;
                staminaTimer = 0;
            }
        }
    }

    public override void ApplyDamage(float amount, bool ignoreArmor, float multiplier)
    {
        if (armor > 0 && !ignoreArmor)
        {
            armor--;
            return;
        }

        float damage = amount * invincible * damageMultiplier * multiplier;

        if (damage > 0)
        {
            OnHurt();
        }

        health = Mathf.Max(0, health - damage);

        if (internalized)
        {
            internalizedDamage += damage * maxInternalizedDamageRate;
        }
        else
        {
            internalizedDamage = 0;
        }

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public override void ApplyHeal(float amount)
    {
        health = Mathf.Min(maxHealth, health + internalizedDamage + amount * decayed);
        internalizedDamage = 0;
    }

    protected override void OnHurt()
    {
        Effect invincibilityEffect = new InvincibilityStatusEffect(frame: true, rate: 0.1f, duration: invincibilityFrames, source: this, allowedTags: EntityTag.Player);
        invincibilityEffect.OnEnter(this);
    }

    public void ToggleInternalized(bool flag)
    {
        internalized = flag;
    }
}

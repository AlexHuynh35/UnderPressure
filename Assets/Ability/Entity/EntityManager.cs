using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Entity Metadata")]
    public EntityTag tags;

    [Header("Cached Components")]
    public Rigidbody2D rb;
    public AbilityManager abilityManager;
    public EffectManager effectManager;
    public SpriteManager spriteManager;
    public Inventory inventory;
    public Vector2 orientation;

    [Header("Combat Stats")]
    public float health;
    public int armor;
    public float[] speeds;
    public float speed;
    public float proficiency;
    public float attackMultiplier;
    public float damageMultiplier;
    public int stunned;
    public int silenced;
    public int decayed;
    public int confused;

    [Header("Default Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxProficiency;
    [SerializeField] private float maxAttackMultiplier;
    [SerializeField] private float maxDamageMultiplier;

    [Header("Stamina Stats")]
    public int stamina;
    public float staminaRate;
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxStaminaRate;
    private float staminaTimer;


    /* Lifecycle */
    void Start()
    {
        speeds = new float[3];
        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = 1;
        }
    }

    void Update()
    {
        RegainStamina();
    }

    /* Combat */
    public void ApplyDamage(float amount, bool ignoreArmor, float multiplier)
    {
        if (armor > 0 && !ignoreArmor)
        {
            armor--;
            return;
        }

        health = Mathf.Max(0, health - amount * damageMultiplier * multiplier);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void ApplyPercentDamage(float amount)
    {
        health = Mathf.Max(0, health - maxHealth * amount);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    public void ApplyHeal(float amount)
    {
        health = Mathf.Min(maxHealth, health + amount * decayed);
    }

    public void ChangeMovement(float amount, int index)
    {
        speeds[index] = amount;
        speed = maxSpeed * speeds[0] * speeds[1] * speeds[2];
    }

    public void ChangeProficiency(float amount)
    {
        proficiency = maxProficiency * amount;
    }

    public void ChangeAttack(float amount)
    {
        attackMultiplier = maxAttackMultiplier * amount;
    }

    public void ChangeDamage(float amount)
    {
        damageMultiplier = maxDamageMultiplier * amount;
    }

    public void ToggleStun(bool flag)
    {
        if (flag)
        {
            stunned = 0;
        }
        else
        {
            stunned = 1;
        }
    }

    public void ToggleSilence(bool flag)
    {
        if (flag)
        {
            silenced = 0;
        }
        else
        {
            silenced = 1;
        }
    }

    public void ToggleDecay(bool flag)
    {
        if (flag)
        {
            decayed = 0;
        }
        else
        {
            decayed = 1;
        }
    }

    public void ToggleConfuse(bool flag)
    {
        if (flag)
        {
            confused = -1;
        }
        else
        {
            confused = 1;
        }
    }

    public void RegainStamina()
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
}

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

    [Header("Combat Stats")]
    public float health;
    public float maxHealth;
    public int armor;
    public float[] speeds;
    public float speed;
    public float maxSpeed;
    public float proficiency;
    public float maxProficiency;
    public int stunned;
    public int silenced;
    public int decayed;
    public Vector2 orientation;

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

    }

    /* Combat */
    public void ApplyDamage(float amount, bool ignoreArmor)
    {
        if (armor > 0 && !ignoreArmor)
        {
            armor--;
            return;
        }

        health = Mathf.Max(0, health - amount);

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
}

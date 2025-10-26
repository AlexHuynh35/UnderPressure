using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Entity Metadata")]
    public ObjectTag tags;

    [Header("Cached Components")]
    public Rigidbody2D rb;

    [Header("Combat Stats")]
    public float health;
    public float maxHealth;
    public float speed;
    public float maxSpeed;
    public int stunned;
    public int silenced;

    /* Lifecycle */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    /* Combat */
    public void ApplyDamage(float amount)
    {
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
        Destroy(this);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    public void ApplyHeal(float amount, float capMultiplier)
    {
        health = Mathf.Max(health, Mathf.Min(maxHealth * capMultiplier, health + amount));
    }

    public void ChangeMovement(float amount)
    {
        speed = maxSpeed * amount;
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
}

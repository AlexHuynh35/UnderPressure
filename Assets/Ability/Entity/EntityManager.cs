using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Entity Metadata")]
    public EntityTag tags;

    [Header("Cached Components")]
    public Rigidbody2D rb;
    public AbilityManager abilityManager;
    public EffectManager effectManager;

    [Header("Combat Stats")]
    public float health;
    public float maxHealth;
    public int armor;
    public float speed;
    public float maxSpeed;
    public int stunned;
    public int silenced;
    public Vector2 orientation;

    /* Lifecycle */
    void Start()
    {

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
        Destroy(this);
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
        health = Mathf.Min(maxHealth, health + amount);
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

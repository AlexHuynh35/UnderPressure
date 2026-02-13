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
    public int armor;
    public float[] speeds;
    public float speed;
    public float proficiency;
    public float attackMultiplier;
    public float damageMultiplier;
    public int invincible;
    public int stunned;
    public int silenced;
    public int decayed;
    public int confused;

    [Header("Default Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float maxProficiency;
    [SerializeField] protected float maxAttackMultiplier;
    [SerializeField] protected float maxDamageMultiplier;

    [Header("Orientation")]
    public Vector2 orientation;
    public Vector2 movementDirection;
    public bool rotate;

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

    /* Orientation */
    public void ChangeOrientation(Vector2 orientation)
    {
        this.orientation = orientation;
    }

    public void ChangeMovementDirection(Vector2 movementDirection)
    {
        this.movementDirection = movementDirection;
    }

    public void ToggleRotate(bool rotate)
    {
        movementDirection = orientation;
        this.rotate = rotate;
    }

    /* Combat */
    public virtual void ApplyDamage(float amount, bool ignoreArmor, float multiplier)
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

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void ApplyPercentDamage(float amount)
    {
        float damage = maxHealth * amount * invincible;

        if (damage > 0)
        {
            OnHurt();
        }

        health = Mathf.Max(0, health - damage);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath() { }

    protected virtual void OnHurt() { }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    public virtual void ApplyHeal(float amount)
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

    public void ToggleInvincibility(bool flag)
    {
        if (flag)
        {
            invincible = 0;
        }
        else
        {
            invincible = 1;
        }
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
}

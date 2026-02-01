using UnityEngine;

public class PlayerEntityManager : EntityManager
{
    [Header("Stamina Stats")]
    public int stamina;
    public float staminaRate;
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxStaminaRate;
    private float staminaTimer;

    [Header("Invincibility Stats")]
    public float frames;

    [Header("Parry Stats")]
    public bool parried;

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

    protected override void OnHurt()
    {
        Effect invincibilityEffect = new InvincibilityStatusEffect(frame: true, rate: 0.1f, duration: frames, source: this, allowedTags: EntityTag.Player);
        invincibilityEffect.OnEnter(this);
    }

    public bool UseParry()
    {
        if (parried)
        {
            parried = false;
            return true;
        }
        return false;
    }

    public void ToggleParry(bool flag)
    {
        parried = flag;
    }
}

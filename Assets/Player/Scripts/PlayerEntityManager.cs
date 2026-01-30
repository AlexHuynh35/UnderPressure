using UnityEngine;

public class PlayerEntityManager : EntityManager
{
    [Header("Stamina Stats")]
    public int stamina;
    public float staminaRate;
    [SerializeField] private float maxStamina;
    [SerializeField] private float maxStaminaRate;
    private float staminaTimer;

    void Update()
    {
        RegainStamina();
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

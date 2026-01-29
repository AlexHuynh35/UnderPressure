using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private MovementPattern pattern;
    private EntityManager enemy;
    private bool moving;
    private float activeTimer;
    private float cooldownTimer;

    void Start()
    {
        enemy = GetComponent<EntityManager>();
        moving = false;
        activeTimer = pattern.activeTime;
        cooldownTimer = pattern.cooldownTime;
    }

    void Update()
    {
        pattern.SetOrientation(enemy);

        if (moving)
        {
            if (activeTimer > 0)
            {
                activeTimer -= Time.deltaTime;
                pattern.MoveBody(enemy);
            }
            else
            {
                if (Random.value < 0.5) moving = false;
                activeTimer = pattern.activeTime;
                cooldownTimer = pattern.cooldownTime;
            }
        }
        else
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                if (Random.value < 0.75) moving = true;
                cooldownTimer = pattern.activeTime;
                cooldownTimer = pattern.cooldownTime;
            }
        }
    }
}

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    public float activeTime;
    public float cooldownTime;

    protected EntityManager enemy;
    protected bool moving;
    protected float activeTimer;
    protected float cooldownTimer;

    void Start()
    {
        enemy = GetComponent<EntityManager>();
        moving = false;
        activeTimer = activeTime;
        cooldownTimer = cooldownTime;
        Initialize();
    }

    protected virtual void Initialize()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void SetOrientation(EntityManager entity)
    {
        
    }
    public virtual void MoveBody(EntityManager entity)
    {
        
    }
}

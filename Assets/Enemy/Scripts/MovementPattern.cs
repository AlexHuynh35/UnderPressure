using UnityEngine;

public class MovementPattern : ScriptableObject
{
    [Header("Movement Metadata")]
    public new string name;

    [Header("Movement Stats")]
    public float speed;
    public float range;

    public virtual void SetOrientation(EntityManager entity)
    {
        
    }
    public virtual void MoveBody(EntityManager entity)
    {
        
    }
}

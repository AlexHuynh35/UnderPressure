using UnityEngine;

public abstract class HitboxMovement
{
    public HitboxMovement() { }

    public virtual void Move(HitboxManager manager, float deltaTime) { }
}

public class StationaryMovement : HitboxMovement
{
    public StationaryMovement() : base() { }
}

public class StraightMovement : HitboxMovement
{
    private float speed;
    private Vector2 direction;

    public StraightMovement(float speed, Vector2 direction) : base()
    {
        this.speed = speed;
        this.direction = direction;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        manager.transform.position += (Vector3)(direction * speed * deltaTime);
    }
}

public class FollowMovement : HitboxMovement
{
    public EntityManager following;
    private Vector2 offset;

    public FollowMovement(EntityManager following, Vector2 offset) : base()
    {
        this.following = following;
        this.offset = offset;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        if (following == null)
        {
            manager.DestroyHitbox();
            return;
        }
        
        manager.transform.position = (Vector2)following.transform.position + offset;
    }
}

public class OrbitMovement : HitboxMovement
{
    public EntityManager following;
    private float speed;
    private float radius;
    private float angle;

    public OrbitMovement(EntityManager following, float speed, float radius) : base()
    {
        this.following = following;
        this.speed = speed;
        this.radius = radius;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        if (following == null)
        {
            manager.DestroyHitbox();
            return;
        }

        angle += deltaTime * speed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        manager.transform.position = following.transform.position + offset;
    }
}

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
    private Vector3 direction;

    public StraightMovement(float speed, Vector3 direction) : base()
    {
        this.speed = speed;
        this.direction = direction;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        manager.transform.position += direction * speed * deltaTime;
    }

    public void FlipDirection()
    {
        direction = -direction;
    }
}

public class AccelerateMovement : HitboxMovement
{
    private float speed;
    private float acceleration;
    private Vector3 direction;

    public AccelerateMovement(float speed, float acceleration, Vector3 direction) : base()
    {
        this.speed = speed;
        this.acceleration = acceleration;
        this.direction = direction;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        speed += acceleration * deltaTime;
        manager.transform.position += direction * speed * deltaTime;
    }
}

public class BoomerangMovement : HitboxMovement
{
    private float speed;
    private float acceleration;
    private Vector3 direction;

    public BoomerangMovement(float speed, float time, Vector3 direction) : base()
    {
        this.speed = speed;
        this.direction = direction;

        acceleration = 2f * speed / time;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        speed -= acceleration * deltaTime;
        manager.transform.position += direction * speed * deltaTime;
    }
}

public class FixedMovement : HitboxMovement
{
    private float speed;
    private float rate;
    private Vector3 direction;
    private float timer;

    public FixedMovement(float speed, float rate, Vector3 direction) : base()
    {
        this.speed = speed;
        this.rate = rate;
        this.direction = direction;
        timer = rate;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        timer -= deltaTime;
        if (timer < 0)
        {
            manager.transform.position += direction * speed;
            timer = rate;
        }
    }
}

public class FollowMovement : HitboxMovement
{
    public EntityManager following;
    private Vector3 offset;

    public FollowMovement(EntityManager following, Vector3 offset) : base()
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
        
        manager.transform.position = following.transform.position + offset;
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
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        manager.transform.position = following.transform.position + offset;
    }
}

public class ArchMovement : HitboxMovement
{
    private float speed;
    private float height;
    private float distance;
    private Vector3 endLocation;
    private Vector3 direction;

    public ArchMovement(float speed, float height, Vector3 startLocation, Vector3 endLocation) : base()
    {
        this.speed = speed;
        this.height = height;
        Vector3 adjustedStartLocation = new Vector3(startLocation.x, 0, startLocation.z);
        Vector3 adjustedEndLocation = new Vector3(endLocation.x, 0, endLocation.z);
        this.endLocation = adjustedEndLocation;
        distance = Vector3.Distance(adjustedStartLocation, adjustedEndLocation);
        direction = AbilityHelper.GetDirection(adjustedStartLocation, adjustedEndLocation).normalized;
    }

    public override void Move(HitboxManager manager, float deltaTime)
    {
        Vector3 positionAdjustment = direction * speed * deltaTime;
        manager.transform.position = new Vector3(manager.transform.position.x + positionAdjustment.x, manager.transform.position.y + Mathf.Sin(180 * Mathf.Deg2Rad * Vector2.Distance(manager.transform.position, endLocation) / distance) * height, manager.transform.position.z + positionAdjustment.z);
    }
}

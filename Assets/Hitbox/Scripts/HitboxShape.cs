using UnityEngine;

public abstract class HitboxShape
{
    public HitboxShape() { }

    public virtual void SetShape(Collider collider, SpriteRenderer renderer) { }

    public virtual bool IsWithin(Vector3 hitboxPos, Vector3 targetPos)
    {
        return true;
    }
}

public class SphereShape : HitboxShape
{
    public float radius;

    public SphereShape(float radius) : base()
    {
        this.radius = radius;
    }

    public override void SetShape(Collider collider, SpriteRenderer renderer)
    {
        var sphere = collider as SphereCollider;
        if (sphere != null)
        {
            sphere.radius = radius;
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(radius * 2f, radius * 2f);
        }
    }

    public override bool IsWithin(Vector3 hitboxPos, Vector3 targetPos)
    {
        return true;
    }
}

public class BoxShape : HitboxShape
{
    public float x;
    public float y;
    public float z;

    public BoxShape(float x, float y, float z) : base()
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override void SetShape(Collider collider, SpriteRenderer renderer)
    {
        var box = collider as BoxCollider;
        if (box != null)
        {
            box.size = new Vector3(x, y, z);
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(x, z);
        }
    }

    public override bool IsWithin(Vector3 hitboxPos, Vector3 targetPos)
    {
        return true;
    }
}

public class DiskShape : HitboxShape
{
    public float radius;
    public float height;

    public DiskShape(float radius, float height) : base()
    {
        this.radius = radius;
        this.height = height;
    }

    public override void SetShape(Collider collider, SpriteRenderer renderer)
    {
        var disk = collider as CapsuleCollider;
        if (disk != null)
        {
            disk.radius = radius;
            disk.height = height + radius * 2f;
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(radius * 2f, radius * 2f);
        }
    }

    public override bool IsWithin(Vector3 hitboxPos, Vector3 targetPos)
    {
        return Mathf.Abs(hitboxPos.y - targetPos.y) < height / 2f;
    }
}

public class ConeShape : HitboxShape
{
    public float radius;
    public float height;
    public float angle;
    public Vector3 direction;

    public ConeShape(float radius, float height, float angle, Vector3 direction) : base()
    {
        this.radius = radius;
        this.height = height;
        this.angle = angle;
        this.direction = direction;
    }

    public override void SetShape(Collider collider, SpriteRenderer renderer)
    {
        var disk = collider as CapsuleCollider;
        if (disk != null)
        {
            disk.radius = radius;
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(radius * 2f, radius * 2f);
        }
    }

    public override bool IsWithin(Vector3 hitboxPos, Vector3 targetPos)
    {
        Vector3 dirHitboxToEnemy = targetPos - hitboxPos;
        float angleHitboxToEnemy = Mathf.Atan2(dirHitboxToEnemy.z, dirHitboxToEnemy.x) * Mathf.Rad2Deg;
        float angleDirection = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        return Mathf.Abs(hitboxPos.y - targetPos.y) < height / 2f && Mathf.Abs(angleHitboxToEnemy - angleDirection) < angle / 2f;
    }
}

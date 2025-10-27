using UnityEngine;

public abstract class HitboxShape
{
    public HitboxShape() { }

    public virtual void SetShape(Collider2D collider, SpriteRenderer renderer) { }

    public virtual bool IsWithin(Vector2 hitboxPos, Vector2 targetPos)
    {
        return true;
    }
}

public class CircleShape : HitboxShape
{
    public float radius;

    public CircleShape(float radius) : base()
    {
        this.radius = radius;
    }

    public override void SetShape(Collider2D collider, SpriteRenderer renderer)
    {
        var circle = collider as CircleCollider2D;
        if (circle != null)
        {
            circle.radius = radius;
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(radius * 2f, radius * 2f);
        }
    }

    public override bool IsWithin(Vector2 hitboxPos, Vector2 targetPos)
    {
        return true;
    }
}

public class BoxShape : HitboxShape
{
    public float x;
    public float y;

    public BoxShape(float x, float y) : base()
    {
        this.x = x;
        this.y = y;
    }

    public override void SetShape(Collider2D collider, SpriteRenderer renderer)
    {
        var box = collider as BoxCollider2D;
        if (box != null)
        {
            box.size = new Vector2(x, y);
        }
        if (renderer != null)
        {
            renderer.transform.localScale = new Vector2(x, y);
        }
    }

    public override bool IsWithin(Vector2 hitboxPos, Vector2 targetPos)
    {
        return true;
    }
}

public class ConeShape : HitboxShape
{
    public float radius;
    public float angle;
    public Vector2 direction;

    public ConeShape(float radius, float angle, Vector2 direction) : base()
    {
        this.radius = radius;
        this.angle = angle;
        this.direction = direction;
    }

    public override void SetShape(Collider2D collider, SpriteRenderer renderer)
    {
        var circle = collider as CircleCollider2D;
        if (circle != null)
        {
            circle.radius = radius;
        }
        else
        {
            Debug.LogError("ConeShape needs a CircleCollider2D!");
        }
    }

    public override bool IsWithin(Vector2 hitboxPos, Vector2 targetPos)
    {
        Vector2 dirHitboxToEnemy = targetPos - hitboxPos;
        float angleHitboxToEnemy = Mathf.Atan2(dirHitboxToEnemy.y, dirHitboxToEnemy.x) * Mathf.Rad2Deg;
        float angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angleHitboxToEnemy < angleDirection + angle / 2 && angleHitboxToEnemy > angleDirection - angle / 2;
    }
}

using UnityEngine;

public class KnightEnemyMovement : EnemyMovement
{
    [Header("Knight Movement Stats")]
    public float startRange;
    public float endRange;

    void Update()
    {
        SetOrientation(enemy);

        if (moving)
        {
            if (activeTimer > 0)
            {
                activeTimer -= Time.deltaTime;
                MoveBody(enemy);
            }
            else
            {
                if (Random.value < 0.5) moving = false;
                activeTimer = activeTime;
                cooldownTimer = cooldownTime;
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
                cooldownTimer = activeTime;
                cooldownTimer = cooldownTime;
            }
        }
    }

    public override void SetOrientation(EntityManager entity)
    {
        var distance = Vector2.Distance(entity.transform.position, PlayerData.Player.transform.position);
        Vector2 direction = AbilityHelper.GetDirection(entity.transform.position, PlayerData.Player.transform.position);
        if (distance > endRange)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                entity.orientation = direction.x > 0 ? Vector2.right : Vector2.left;
                entity.movementDirection = entity.orientation;
            }
            else
            {
                entity.orientation = direction.y > 0 ? Vector2.up : Vector2.down;
                entity.movementDirection = entity.orientation;
            }
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                entity.orientation = direction.x > 0 ? Vector2.right : Vector2.left;
                entity.movementDirection = direction.y > 0 ? Vector2.up : Vector2.down;
            }
            else
            {
                entity.orientation = direction.y > 0 ? Vector2.up : Vector2.down;
                entity.movementDirection = direction.x > 0 ? Vector2.right : Vector2.left;
            }
        }
    }

    public override void MoveBody(EntityManager entity)
    {
        var distance = Vector2.Distance(entity.transform.position, PlayerData.Player.transform.position);
        Vector2 difference = AbilityHelper.GetDifference(entity.transform.position, PlayerData.Player.transform.position);
        if (distance < startRange)
        {
            if (distance > endRange || (Mathf.Abs(difference.x) > 0.5f && Mathf.Abs(difference.y) > 0.5f))
            {
                entity.rb.AddForce(entity.movementDirection * entity.speed * entity.stunned, ForceMode2D.Force);
                return;
            }
        }
    }
}

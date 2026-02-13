using UnityEngine;

public class KnightEnemyMovement : EnemyMovement
{
    [Header("Knight Movement Stats")]
    public float startRange;
    public float endRange;

    void Update()
    {
        SetOrientation();

        if (enemy.rotate)
        {
            MoveBody();
            return;
        }

        if (moving)
        {
            if (activeTimer > 0)
            {
                activeTimer -= Time.deltaTime;
                MoveBody();
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

    public override void SetOrientation()
    {
        var distance = Vector2.Distance(enemy.transform.position, PlayerData.Player.transform.position);
        Vector2 direction = AbilityHelper.GetDirection(enemy.transform.position, PlayerData.Player.transform.position);
        Vector2 newOrientation;
        Vector2 newMovementDirection;
        if (distance > endRange)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                newOrientation = direction.x > 0 ? Vector2.right : Vector2.left;
                newMovementDirection = newOrientation;
            }
            else
            {
                newOrientation = direction.y > 0 ? Vector2.up : Vector2.down;
                newMovementDirection = newOrientation;
            }
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                newOrientation = direction.x > 0 ? Vector2.right : Vector2.left;
                newMovementDirection = direction.y > 0 ? Vector2.up : Vector2.down;
            }
            else
            {
                newOrientation = direction.y > 0 ? Vector2.up : Vector2.down;
                newMovementDirection = direction.x > 0 ? Vector2.right : Vector2.left;
            }
        }

        enemy.ChangeOrientation(newOrientation);
        if (!enemy.rotate)
        {
            enemy.ChangeMovementDirection(newMovementDirection);
        }
        else
        {
            float currentAngle = Mathf.Atan2(enemy.movementDirection.y, enemy.movementDirection.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(newMovementDirection.y, newMovementDirection.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, enemy.speed * Time.fixedDeltaTime) * Mathf.Deg2Rad;
            enemy.ChangeMovementDirection(new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle)).normalized);
        }
    }

    public override void MoveBody()
    {
        var distance = Vector2.Distance(enemy.transform.position, PlayerData.Player.transform.position);
        Vector2 difference = AbilityHelper.GetDifference(enemy.transform.position, PlayerData.Player.transform.position);
        if (distance < startRange)
        {
            if (distance > endRange || (Mathf.Abs(difference.x) > 0.5f && Mathf.Abs(difference.y) > 0.5f))
            {
                enemy.rb.AddForce(enemy.movementDirection * enemy.speed * enemy.stunned, ForceMode2D.Force);
            }
        }
    }
}

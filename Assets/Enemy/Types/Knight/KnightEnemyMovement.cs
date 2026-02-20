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
        Vector3 enemyTransform = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 playerTransform = new Vector3(PlayerData.Player.transform.position.x, 0, PlayerData.Player.transform.position.z);
        var distance = Vector3.Distance(enemyTransform, playerTransform);
        Vector3 direction = AbilityHelper.GetDirection(enemyTransform, playerTransform);
        Vector3 newOrientation;
        Vector3 newMovementDirection;
        if (distance > endRange)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                newOrientation = direction.x > 0 ? Vector3.right : Vector3.left;
                newMovementDirection = newOrientation;
            }
            else
            {
                newOrientation = direction.z > 0 ? Vector3.forward : Vector3.back;
                newMovementDirection = newOrientation;
            }
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                newOrientation = direction.x > 0 ? Vector3.right : Vector3.left;
                newMovementDirection = direction.z > 0 ? Vector3.forward : Vector3.back;
            }
            else
            {
                newOrientation = direction.z > 0 ? Vector3.forward : Vector3.back;
                newMovementDirection = direction.x > 0 ? Vector3.right : Vector3.left;
            }
        }

        enemy.ChangeOrientation(newOrientation);
        if (!enemy.rotate)
        {
            enemy.ChangeMovementDirection(newMovementDirection.normalized);
        }
        else
        {
            float currentAngle = Mathf.Atan2(enemy.movementDirection.z, enemy.movementDirection.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(newMovementDirection.z, newMovementDirection.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, enemy.rotateSpeed * Time.fixedDeltaTime) * Mathf.Deg2Rad;
            enemy.ChangeMovementDirection(new Vector3(Mathf.Cos(newAngle), 0, Mathf.Sin(newAngle)).normalized);
        }
    }

    public override void MoveBody()
    {
        Vector3 enemyTransform = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 playerTransform = new Vector3(PlayerData.Player.transform.position.x, 0, PlayerData.Player.transform.position.z);
        var distance = Vector3.Distance(enemyTransform, playerTransform);
        Vector3 difference = AbilityHelper.GetDifference(enemyTransform, playerTransform);
        if (distance < startRange)
        {
            if (distance > endRange || (Mathf.Abs(difference.x) > 0.5f && Mathf.Abs(difference.z) > 0.5f))
            {
                enemy.rb.AddForce(enemy.movementDirection * enemy.speed * enemy.stunned, ForceMode.Force);
            }
        }
    }
}

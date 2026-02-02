using UnityEngine;

[CreateAssetMenu(fileName = "KnightMovementPattern", menuName = "MovementPattern/KnightMovementPattern")]
public class KnightMovementPattern : MovementPattern
{
    public float startRange;
    public float endRange;

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

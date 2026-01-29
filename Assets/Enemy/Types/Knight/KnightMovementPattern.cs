using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KnightMovementPattern", menuName = "MovementPattern/KnightMovementPattern")]
public class KnightMovementPattern : MovementPattern
{
    public override void SetOrientation(EntityManager entity)
    {
        Vector2 direction = AbilityHelper.GetDirection(entity.transform.position, PlayerData.Player.transform.position);
        if (direction != Vector2.zero)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                entity.orientation = direction.x > 0 ? Vector2.right : Vector2.left;
            else
                entity.orientation = direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    public override void MoveBody(EntityManager entity)
    {
        
    }
}

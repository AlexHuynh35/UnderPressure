using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum Direction
{
    Up, Down, Left, Right
}

public static class DirectionDatabase
{
    public static Dictionary<Direction, (int, int)> directionToPosition = new Dictionary<Direction, (int, int)>
    {
        {Direction.Up, (0, 1)},
        {Direction.Down, (0, -1)},
        {Direction.Left, (-1, 0)},
        {Direction.Right, (1, 0)}
    };

    public static Direction GetOpposite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        
        return Direction.Up;
    }
}

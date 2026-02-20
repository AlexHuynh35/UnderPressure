using UnityEngine;

public static class AbilityHelper
{
    public static Vector2 AddOffset(Vector2 direction, float angle, float index)
    {
        float rad = angle * index * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector2 newDir = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );

        return newDir.normalized;
    }

    public static Vector2 GetDirection(Vector2 startingPoint, Vector2 endingPoint)
    {
        return (endingPoint - startingPoint).normalized;
    }

    public static Vector2 GetDifference(Vector2 startingPoint, Vector2 endingPoint)
    {
        return endingPoint - startingPoint;
    }

    public static Vector3 OffsetLocation(Vector3 location, float offset)
    {
        return new Vector3(location.x + Random.Range(-offset, offset), location.y, location.z + Random.Range(-offset, offset));
    }
}

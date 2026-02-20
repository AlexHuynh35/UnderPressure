using UnityEngine;

public static class AbilityHelper
{
    public static Vector3 AddOffset(Vector3 direction, float angle, float index)
    {
        float rad = angle * index * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector3 newDir = new Vector3(direction.x * cos - direction.z * sin, 0, direction.x * sin + direction.z * cos);

        return newDir.normalized;
    }

    public static Vector3 GetDirection(Vector3 startingPoint, Vector3 endingPoint)
    {
        Vector3 difference = endingPoint - startingPoint;
        difference.y = 0;
        return difference.normalized;
    }

    public static Vector3 GetDifference(Vector3 startingPoint, Vector3 endingPoint)
    {
        Vector3 difference = endingPoint - startingPoint;
        difference.y = 0;
        return difference;
    }

    public static Vector3 OffsetLocation(Vector3 location, float offset)
    {
        return new Vector3(location.x + Random.Range(-offset, offset), location.y, location.z + Random.Range(-offset, offset));
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDatabase", menuName = "RoomDatabase")]
public class RoomDatabase : ScriptableObject
{
    public List<GameObject> centerRooms;
    public List<GameObject> topRooms;
    public List<GameObject> bottomRooms;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;

    private List<GameObject> allRooms;

    private void OnEnable()
    {
        List<GameObject> allRooms = new List<GameObject>();
        allRooms.Union(topRooms);
        allRooms.Union(bottomRooms);
        allRooms.Union(leftRooms);
        allRooms.Union(rightRooms);
    }

    public List<GameObject> GetFittingRooms(bool top, bool bottom, bool left, bool right)
    {
        List<GameObject> rooms = new List<GameObject>(allRooms);

        if (top)
        {
            rooms.Intersect(topRooms);
        }
        if (bottom)
        {
            rooms.Intersect(bottomRooms);
        }
        if (left)
        {
            rooms.Intersect(leftRooms);
        }
        if (right)
        {
            rooms.Intersect(rightRooms);
        }

        rooms.AddRange(centerRooms);

        return rooms;
    }
}

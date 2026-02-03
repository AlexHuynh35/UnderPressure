using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDatabase", menuName = "RoomDatabase")]
public class RoomDatabase : ScriptableObject
{
    public GameObject entranceRoom;
    public List<GameObject> centerRooms;
    public List<GameObject> topRooms;
    public List<GameObject> bottomRooms;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;

    private List<GameObject> allRooms;

    private void OnEnable()
    {
        allRooms = new List<GameObject>();
        allRooms = allRooms.Union(topRooms).ToList();
        allRooms = allRooms.Union(bottomRooms).ToList();
        allRooms = allRooms.Union(leftRooms).ToList();
        allRooms = allRooms.Union(rightRooms).ToList();
    }

    public List<GameObject> GetFittingRooms(bool top, bool bottom, bool left, bool right)
    {
        List<GameObject> rooms = new List<GameObject>(allRooms);

        if (top)
        {
            rooms = rooms.Intersect(topRooms).ToList();
        }
        if (bottom)
        {
            rooms = rooms.Intersect(bottomRooms).ToList();
        }
        if (left)
        {
            rooms = rooms.Intersect(leftRooms).ToList();
        }
        if (right)
        {
            rooms = rooms.Intersect(rightRooms).ToList();
        }

        rooms.AddRange(centerRooms);

        return rooms;
    }
}

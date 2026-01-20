using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<DoorManager> doorList;
    public Dictionary<Direction, DoorManager> doorDict = new Dictionary<Direction, DoorManager>();
    [HideInInspector] public bool roomCleared;

    void Start()
    {
        StartRoom();
    }

    void Update()
    {
        DebugClearRoom();
    }

    public void InitializeDoorDict()
    {
        foreach (var door in doorList)
        {
            doorDict[door.direction] = door;
        }
    }

    public void InitializeRoom(RoomStructure structure)
    {
        foreach (var door in structure.rooms)
        {
            if (door.Value < 0)
            {
                doorDict.Remove(door.Key);
            }
            else
            {
                doorDict[door.Key].InitializeDoor(structure.id);
            }
        }
    }

    public void OpenDoors()
    {
        foreach (var door in doorDict)
        {
            door.Value.Open();
        }
    }

    public void CloseDoors()
    {
        foreach (var door in doorList)
        {
            door.Close();
        }
    }

    public void StartRoom()
    {
        roomCleared = false;
        CloseDoors();
    }

    public void ClearRoom()
    {
        roomCleared = true;
        OpenDoors();
    }

    public void DebugClearRoom()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ClearRoom();
        }
    }
}

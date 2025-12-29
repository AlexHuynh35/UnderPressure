using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> doors = new List<GameObject>(4);
    [HideInInspector] public bool roomCleared;

    void Start()
    {
        StartRoom();
    }

    void Update()
    {
        DebugClearRoom();
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door?.SetActive(false);
        }
    }

    public void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door?.SetActive(true);
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

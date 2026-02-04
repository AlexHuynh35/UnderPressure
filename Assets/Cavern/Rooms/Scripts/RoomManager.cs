using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoomRules rules;
    public List<DoorManager> doorList;
    public List<Spawner> spawnerList;
    public Dictionary<Direction, DoorManager> doorDict = new Dictionary<Direction, DoorManager>();
    [HideInInspector] public bool roomCleared;

    private int currentRound;
    private bool roundStart;
    private List<GameObject> enemies = new List<GameObject>();

    private ChestInventory chest;

    void Start()
    {

    }

    void Update()
    {
        CheckRound();
        CheckReward();
        DebugClearRoom();
    }

    public void InitializeRoom(RoomStructure structure, RoomRules rules)
    {
        InitializeDoorDict();

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

        this.rules = rules;

        StartRoom();
    }

    public void InitializeEntranceRoom(RoomStructure structure)
    {
        InitializeDoorDict();

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

        ClearRoom();
    }

    public void InitializeDoorDict()
    {
        foreach (var door in doorList)
        {
            doorDict[door.direction] = door;
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

    public void EnterRoom(Direction direction)
    {
        if (!roomCleared)
        {
            foreach (var door in doorDict)
            {
                if (door.Value.direction == direction)
                {
                    door.Value.Enter();
                }
                else
                {
                    door.Value.Close();
                }
            }

            StartRound();
        }
    }

    public void StartRoom()
    {
        roomCleared = false;
        currentRound = 1;
        roundStart = false;
        enemies.Clear();
        CloseDoors();
    }

    public void ClearRoom()
    {
        roomCleared = true;
        roundStart = false;
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        OpenDoors();
    }

    public void DebugClearRoom()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ClearRoom();
        }
    }

    public void StartRound()
    {
        for (int i = 0; i < rules.GetNumberEnemy(); i++)
        {
            enemies.Add(spawnerList[i % spawnerList.Count].SpawnEnemy(rules.GetRandomEnemy()));
        }

        roundStart = true;
    }

    public void CheckRound()
    {
        if (roundStart)
        {
            enemies.RemoveAll(enemy => enemy == null);
            if (enemies.Count <= 0)
            {
                roundStart = false;
                ClearRound();
            }
        }
    }

    public void ClearRound()
    {
        currentRound++;

        if (currentRound <= rules.rounds)
        {
            StartRound();
        }
        else
        {
            GiveReward();
        }
    }

    public void GiveReward()
    {
        chest = Instantiate(rules.chestPrefab, Vector2.zero, Quaternion.identity, transform).GetComponent<ChestInventory>();
        chest.Initialize();
        for (int i = 0; i < rules.averageNumberRewardTypes; i++)
        {
            chest.AddToInventory(rules.PickRandomReward());
        }
    }

    public void CheckReward()
    {
        if (chest != null && !roomCleared)
        {
            if (chest.opened) ClearRoom();
        }
    }
}

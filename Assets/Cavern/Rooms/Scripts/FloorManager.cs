using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomStructure
{
    public bool visited;
    public int id;

    // property represents the diection of the door and the id of the room that door leads to
    public Dictionary<Direction, int> rooms;
}

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance { get; private set; }
    public int rows;
    public int columns;
    public int numRooms;
    public GameObject roomPrefab;

    private (int, int) startingPoint;
    private RoomStructure[,] floor;
    private Dictionary<int, RoomStructure> roomStructures = new Dictionary<int, RoomStructure>();
    private Dictionary<int, RoomManager> roomManagers = new Dictionary<int, RoomManager>();

    private Queue<(int, int)> roomQueue = new Queue<(int, int)>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        floor = FillFloor();
        startingPoint = (UnityEngine.Random.Range(0, rows), UnityEngine.Random.Range(0, columns));

        if (startingPoint.Item2 > 0)
        {
            RoomStructure belowStartingRoom = floor[startingPoint.Item1, startingPoint.Item2 - 1];
            belowStartingRoom.visited = true;
            floor[startingPoint.Item1, startingPoint.Item2 - 1] = belowStartingRoom;
        }

        RoomStructure startingRoom = floor[startingPoint.Item1, startingPoint.Item2];
        startingRoom.visited = true;
        startingRoom.id = 1;
        startingRoom.rooms[Direction.Down] = 0;
        floor[startingPoint.Item1, startingPoint.Item2] = startingRoom;
        roomQueue.Enqueue(startingPoint);

        CreateFloorStructure();
        CreateRoomStructureDict();
        CreateRoomManagerDict();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Debug.Log(floor[i, j].visited);
                Debug.Log(floor[i, j].id);
                Debug.Log("List contents: " + string.Join(", ", floor[i, j].rooms));
            }
        }

        HUDManager.Instance.SetMap(rows, columns, floor);

        roomManagers[1].gameObject.SetActive(true);
    }

    void Update()
    {

    }

    public void LoadNextRoom(EntityManager entity, Direction direction, int currentRoomID)
    {
        RoomStructure currentRoomStructure = roomStructures[currentRoomID];
        RoomManager currentRoomManager = roomManagers[currentRoomID];
        RoomManager nextRoomManager = roomManagers[currentRoomStructure.rooms[direction]];
        currentRoomManager.gameObject.SetActive(false);
        nextRoomManager.gameObject.SetActive(true);
        nextRoomManager.EnterRoom(DirectionDatabase.GetOpposite(direction));
        entity.transform.position = nextRoomManager.doorDict[DirectionDatabase.GetOpposite(direction)].transform.position;
        HUDManager.Instance.ChangePlayerLocation(currentRoomID, currentRoomStructure.rooms[direction]);
    }

    private RoomStructure[,] FillFloor()
    {
        RoomStructure[,] newFloor = new RoomStructure[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                newFloor[i, j] = CreateRoomStructure();
            }
        }

        return newFloor;
    }

    private RoomStructure CreateRoomStructure()
    {
        return new RoomStructure
        {
            visited = false,
            id = -1,
            rooms = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToDictionary(e => e, e => -1)
        };
    }

    private void CreateFloorStructure()
    {
        int totalRoomIDs = 1;

        while (totalRoomIDs <= numRooms && roomQueue.Count > 0)
        {
            (int, int) currentPosition = roomQueue.Dequeue();
            int oldRoomID = floor[currentPosition.Item1, currentPosition.Item2].id;
            List<Direction> paths = PickBranchingPaths(GetAvailableDirections(currentPosition));

            if (paths.Count > 0)
            {
                foreach (Direction direction in paths)
                {
                    totalRoomIDs++;
                    if (totalRoomIDs <= numRooms)
                    {
                        (int, int) newPosition = (currentPosition.Item1 + DirectionDatabase.directionToPosition[direction].Item1, currentPosition.Item2 + DirectionDatabase.directionToPosition[direction].Item2);
                        RoomStructure newRoom = floor[newPosition.Item1, newPosition.Item2];
                        newRoom.visited = true;
                        newRoom.id = totalRoomIDs;
                        newRoom.rooms[DirectionDatabase.GetOpposite(direction)] = oldRoomID;
                        floor[newPosition.Item1, newPosition.Item2] = newRoom;
                        roomQueue.Enqueue(newPosition);

                        RoomStructure oldRoom = floor[currentPosition.Item1, currentPosition.Item2];
                        oldRoom.rooms[direction] = totalRoomIDs;
                        floor[currentPosition.Item1, currentPosition.Item2] = oldRoom;
                    }
                }
            }
        }
    }

    private List<Direction> GetAvailableDirections((int, int) currentRoom)
    {
        List<Direction> availableDirections = new List<Direction>();

        foreach (var direction in DirectionDatabase.directionToPosition)
        {
            (int, int) position = (currentRoom.Item1 + direction.Value.Item1, currentRoom.Item2 + direction.Value.Item2);
            if (position.Item1 < rows && position.Item1 >= 0 && position.Item2 < columns && position.Item2 >= 0)
            {
                if (!floor[position.Item1, position.Item2].visited)
                {
                    availableDirections.Add(direction.Key);
                }
            }
        }

        return availableDirections;
    }

    private List<Direction> PickBranchingPaths(List<Direction> availableDirections)
    {
        if (availableDirections.Count == 0) return new List<Direction>() { };

        List<Direction> branchingPaths = new List<Direction>() { availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)] };

        for (int i = 0; i < availableDirections.Count; i++)
        {
            if (!branchingPaths.Contains(availableDirections[i]))
            {
                if (UnityEngine.Random.value <= 0.5)
                {
                    branchingPaths.Add(availableDirections[i]);
                }
            }
        }

        return branchingPaths;
    }

    private void CreateRoomStructureDict()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (floor[i, j].visited)
                {
                    roomStructures[floor[i, j].id] = floor[i, j];
                }
            }
        }
    }

    private void CreateRoomManagerDict()
    {
        foreach (var roomStructure in roomStructures)
        {
            RoomManager room = Instantiate(roomPrefab, new Vector3(0, 0, 1), Quaternion.identity, transform).GetComponent<RoomManager>();
            roomManagers[roomStructure.Key] = room;
            room.InitializeRoom(roomStructure.Value);
            room.gameObject.SetActive(false);
        }
    }
}

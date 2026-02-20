using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;

    public RoomRules rules;
    public List<DoorManager> doorList;
    public List<Spawner> spawnerList;
    public DropHolder dropHolder;
    public EnemyHolder enemyHolder;
    public Dictionary<Direction, DoorManager> doorDict = new Dictionary<Direction, DoorManager>();
    [HideInInspector] public bool roomCleared;

    private int currentRound;
    private bool roundStart;
    private List<GameObject> enemies = new List<GameObject>();

    private ChestInventory chest;

    private bool[,] wallGrid;
    private bool[,] visitedGrid;
    private int width;
    private int height;
    private Vector3Int origin;
    private Vector3 center;

    private void Awake()
    {
        ReadTilemap();
    }

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
        InitializeSpawners();

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
        InitializeSpawners();

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

    private void InitializeDoorDict()
    {
        foreach (var door in doorList)
        {
            doorDict[door.direction] = door;
        }
    }

    private void InitializeSpawners()
    {
        foreach (var spawner in spawnerList)
        {
            spawner.Initialize(enemyHolder);
        }
    }

    private void OpenDoors()
    {
        foreach (var door in doorDict)
        {
            door.Value.Open();
        }
    }

    private void CloseDoors()
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

        SetMapObjects();
    }

    public void EnterEntranceRoom()
    {
        SetMapObjects();
    }

    private void StartRoom()
    {
        roomCleared = false;
        currentRound = 1;
        roundStart = false;
        enemies.Clear();
        CloseDoors();
    }

    private void ClearRoom()
    {
        roomCleared = true;
        roundStart = false;
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        OpenDoors();
    }

    private void DebugClearRoom()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ClearRoom();
        }
    }

    private void StartRound()
    {
        for (int i = 0; i < rules.GetNumberEnemy(); i++)
        {
            enemies.Add(spawnerList[i % spawnerList.Count].SpawnEnemy(rules.GetRandomEnemy(), dropHolder));
        }

        roundStart = true;
    }

    private void CheckRound()
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

    private void ClearRound()
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

    private void GiveReward()
    {
        chest = Instantiate(rules.chestPrefab, new Vector3(0, 1, 0), Quaternion.identity, transform).GetComponent<ChestInventory>();
        chest.Initialize(dropHolder);
        for (int i = 0; i < rules.averageNumberRewardTypes; i++)
        {
            chest.AddToInventory(rules.PickRandomReward());
        }
    }

    private void CheckReward()
    {
        if (chest != null && !roomCleared)
        {
            if (chest.opened) ClearRoom();
        }
    }

    private void ReadTilemap()
    {
        floorTilemap.CompressBounds();
        wallTilemap.CompressBounds();

        BoundsInt bounds = wallTilemap.cellBounds;

        width = bounds.size.x;
        height = bounds.size.y;
        origin = bounds.min;

        Vector3 min = wallTilemap.CellToWorld(bounds.min);
        Vector3 max = wallTilemap.CellToWorld(bounds.max);
        center = (min + max) / 2f;

        wallGrid = new bool[width, height];
        visitedGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPos = new Vector3Int(x + origin.x, y + origin.y, 0);
                TileBase tile = wallTilemap.GetTile(cellPos);
                wallGrid[x, y] = tile != null;
            }
        }
    }

    private void SetMapObjects()
    {
        MapObjectPool.Instance.ResetWalls();
        MapObjectPool.Instance.UseFloor(new Vector3(center.x, transform.position.y - 0.5f, center.z), new Vector3(width, 1, height));

        visitedGrid = new bool[width, height];

        SetHorizontalWall();
        SetVerticalWall();
    }

    private void SetHorizontalWall()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (wallGrid[x, z] && !visitedGrid[x, z])
                {
                    int length = 1;

                    while (x + length < width && wallGrid[x + length, z] && !visitedGrid[x + length, z])
                    {
                        length++;
                    }

                    if (length > 1)
                    {
                        Vector3 centerCellPos = wallTilemap.GetCellCenterWorld(new Vector3Int(x + origin.x, z + origin.y, 0));

                        MapObjectPool.Instance.UseWall(new Vector3(centerCellPos.x + (length - 1) / 2f, 0.5f, centerCellPos.z), new Vector3(length, 1, 1));

                        for (int i = 0; i < length; i++)
                        {
                            visitedGrid[x + i, z] = true;
                        }

                        x += length - 1;
                    }
                }
            }
        }
    }

    private void SetVerticalWall()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (wallGrid[x, z] && !visitedGrid[x, z])
                {
                    int length = 1;

                    while (z + length < height && wallGrid[x, z + length] && !visitedGrid[x, z + length])
                    {
                        length++;
                    }

                    Vector3 centerCellPos = wallTilemap.GetCellCenterWorld(new Vector3Int(x + origin.x, z + origin.y, 0));

                    MapObjectPool.Instance.UseWall(new Vector3(centerCellPos.x, 0.5f, centerCellPos.z + (length - 1) / 2f), new Vector3(1, 1, length));

                    for (int i = 0; i < length; i++)
                    {
                        visitedGrid[x, z + i] = true;
                    }
                }
            }
        }
    }
}

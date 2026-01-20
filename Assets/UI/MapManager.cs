using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public float xSpacing;
    public float ySpacing;
    public GameObject cellPrefab;
    private Dictionary<int, MapCellManager> cellManagers = new Dictionary<int, MapCellManager>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetMap(int rows, int columns, RoomStructure[, ] structures)
    {
        int halfRows = rows / 2;
        int halfColumns = columns / 2;
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (structures[i, j].visited && structures[i, j].id > 0)
                {
                    Vector2 position = new Vector2 (transform.position.x + (i - halfRows) * xSpacing, transform.position.y + (j - halfColumns) * ySpacing);
                    cellManagers[structures[i, j].id] = Instantiate(cellPrefab, position, Quaternion.identity, transform).GetComponent<MapCellManager>();
                    cellManagers[structures[i, j].id].Initialize();
                    cellManagers[structures[i, j].id].SetRoomID(structures[i, j].id);
                    foreach (var room in structures[i, j].rooms)
                    {
                        cellManagers[structures[i, j].id].SetDoor(room.Key, room.Value > 0);
                    }
                }
            }
        }

        cellManagers[1].SetPlayer(true);
    }

    public void ChangePlayerLocation(int current, int next)
    {
        cellManagers[current].SetPlayer(false);
        cellManagers[next].SetPlayer(true);
    }
}

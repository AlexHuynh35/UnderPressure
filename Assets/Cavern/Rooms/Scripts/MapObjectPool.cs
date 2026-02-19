using UnityEngine;
using System.Collections.Generic;

public class MapObjectPool : MonoBehaviour
{
    public static MapObjectPool Instance { get; private set; }
    [SerializeField] private GameObject floor;
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private GameObject wallPrefab;
    
    private int nextAvailableWall;
    
    private void Awake()
    {
        Instance = this;
        nextAvailableWall = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UseFloor(Vector3 position, Vector3 size)
    {
        floor.transform.position = position;
        floor.transform.localScale = size;
    }

    public void UseWall(Vector3 position, Vector3 size)
    {
        if (nextAvailableWall == walls.Count)
        {
            CreateWall(position, size);
            nextAvailableWall++;
        }
        else
        {
            walls[nextAvailableWall].transform.position = position;
            walls[nextAvailableWall].transform.localScale = size;
            nextAvailableWall++;
        }
    }

    private void CreateWall(Vector3 position, Vector3 size)
    {
        GameObject newWall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        newWall.transform.localScale = size;
        walls.Add(newWall);
    }

    public void ResetWalls()
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
        }
        nextAvailableWall = 0;
    }
}

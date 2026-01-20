using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    public MapManager map;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        map.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            map.gameObject.SetActive(!map.gameObject.activeSelf);
        }
    }
    
    public void SetMap(int rows, int columns, RoomStructure[, ] structures)
    {
        map.SetMap(rows, columns, structures);
    }
}

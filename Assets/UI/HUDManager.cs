using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    public GameObject menu;

    [Header("Navigation")]
    public Button inventoryButton;
    public Button mapButton;

    [Header("Inventory")]
    public GameObject inventoryContainer;

    [Header("Map")]
    public GameObject mapContainer;
    private MapManager map;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        map = mapContainer.GetComponentInChildren<MapManager>();
        inventoryButton.onClick.AddListener(OpenInventory);
        mapButton.onClick.AddListener(OpenMap);
        OpenInventory();
        menu.SetActive(false);
    }

    void Update()
    {
        if (InputManager.Instance.PressMenu())
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
    
    public void SetMap(int rows, int columns, RoomStructure[, ] structures)
    {
        map.SetMap(rows, columns, structures);
    }

    public void ChangePlayerLocation(int current, int next)
    {
        map.ChangePlayerLocation(current, next);
    }

    private void OpenInventory()
    {
        mapContainer.SetActive(false);
        inventoryContainer.SetActive(true);
    }

    private void OpenMap()
    {
        inventoryContainer.SetActive(false);
        mapContainer.SetActive(true);
    }
}

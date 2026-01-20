using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject blockage;
    [SerializeField] private GameObject door;
    public Direction direction;
    private int roomID;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager entity = other.gameObject.GetComponent<EntityManager>();
        if ((entity.tags & EntityTag.Player) != 0)
        {
            FloorManager.Instance.LoadNextRoom(entity, direction, roomID);
        }
    }

    public void InitializeDoor(int roomID)
    {
        this.roomID = roomID;
        door.transform.position = new Vector2(blockage.transform.position.x + DirectionDatabase.directionToPosition[direction].Item1 * 2.5f, blockage.transform.position.y + DirectionDatabase.directionToPosition[direction].Item2 * 2.5f);
    }

    public void Open()
    {
        blockage.SetActive(false);
        door.SetActive(true);
    }

    public void Close()
    {
        blockage.SetActive(true);
        door.SetActive(false);
    }
}

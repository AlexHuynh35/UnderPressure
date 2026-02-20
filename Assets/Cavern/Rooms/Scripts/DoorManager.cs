using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject blockage;
    [SerializeField] private GameObject door;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;
    public Direction direction;
    private int roomID;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        EntityManager entity = other.gameObject.GetComponent<EntityManager>();
        if (entity != null && (entity.tags & EntityTag.Player) != 0)
        {
            FloorManager.Instance.LoadNextRoom(entity, direction, roomID);
        }
    }

    public void InitializeDoor(int roomID)
    {
        this.roomID = roomID;
        door.transform.position = new Vector3(transform.position.x + DirectionDatabase.directionToPosition[direction].Item1 * 1f, transform.position.y, transform.position.z + DirectionDatabase.directionToPosition[direction].Item2 * 1f);
    }

    public void Enter()
    {
        blockage.SetActive(false);
        door.SetActive(false);
        sprite.sprite = open;
    }

    public void Open()
    {
        blockage.SetActive(false);
        door.SetActive(true);
        sprite.sprite = open;
    }

    public void Close()
    {
        blockage.SetActive(true);
        door.SetActive(false);
        sprite.sprite = close;
    }
}

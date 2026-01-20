using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MapCellManager : MonoBehaviour
{
    public TextMeshProUGUI roomID;
    public Image player;
    public Image topDoor;
    public Image bottomDoor;
    public Image leftDoor;
    public Image rightDoor;
    private Dictionary<Direction, Image> imageDict = new Dictionary<Direction, Image>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        imageDict[Direction.Up] = topDoor;
        imageDict[Direction.Down] = bottomDoor;
        imageDict[Direction.Left] = leftDoor;
        imageDict[Direction.Right] = rightDoor;

        SetPlayer(false);
    }

    public void SetRoomID(int id)
    {
        roomID.SetText(id.ToString());
    }

    public void SetDoor(Direction direction, bool active)
    {
        Color color = imageDict[direction].color;
        color.a = active ? 1 : 0;
        imageDict[direction].color = color;
    }

    public void SetPlayer(bool active)
    {
        Color color = player.color;
        color.a = active ? 1 : 0;
        player.color = color;
    }
}

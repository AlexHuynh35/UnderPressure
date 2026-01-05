using UnityEngine;

public enum ItemType
{
    Arrow,
    Potion,
}

public class Item : ScriptableObject
{
    public ItemType type;
}

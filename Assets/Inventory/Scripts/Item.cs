using UnityEngine;

public enum ItemType
{
    Arrow,
    Potion,
    Weapon,
}

public class Item : ScriptableObject
{
    [Header("Item Metadata")]
    public new string name;
    public ItemType type;
}

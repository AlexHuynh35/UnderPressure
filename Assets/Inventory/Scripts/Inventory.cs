using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStack
{
    public Item item;
    public int amount;

    public ItemType Type => item.type;

    public ItemStack(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public bool IsEmpty => item == null || amount <= 0;
}

public class Inventory : MonoBehaviour
{
    public Arrow arrowInfo;
    public int arrowAmount;
    
    [SerializeField] protected int inventorySpace;
    [SerializeField] protected GameObject dropPrefab;

    [HideInInspector] public Dictionary<Type, ItemStack> inventory = new Dictionary<Type, ItemStack>();

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual bool AddToInventory(ItemStack itemStack)
    {
        return false;
    }

    public virtual void DropItem()
    {
        foreach (var itemStack in inventory)
        {
            ItemDrop drop = Instantiate(dropPrefab, AbilityHelper.OffsetLocation(transform.position, 1), Quaternion.identity).GetComponent<ItemDrop>();
            drop.Initialize(itemStack.Value);
        }
    }
}

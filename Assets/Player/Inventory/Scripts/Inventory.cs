using System;
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

    public Potion potionInfo;
    public int potionAmount;

    [HideInInspector] public ItemStack arrow;
    [HideInInspector] public ItemStack potion;

    void Start()
    {
        arrow = new ItemStack(arrowInfo, arrowAmount);
        potion = new ItemStack(potionInfo, potionAmount);
    }

    void Update()
    {
        
    }
}

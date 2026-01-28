using System;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [HideInInspector] public ItemStack arrow;
    [HideInInspector] public ItemStack potion;
    
    void Start()
    {
        arrow = new ItemStack(arrowInfo, arrowAmount);
    }

    void Update()
    {
        
    }

    public override bool AddToInventory(ItemStack itemStack)
    {
        Type type = itemStack.item.GetType();

        if (type == arrow.item.GetType())
        {
            ItemStack newStack = arrow;
            newStack.amount += itemStack.amount;
            arrow = newStack;
            return true;
        }

        if (type == potion.item.GetType())
        {
            ItemStack newStack = potion;
            newStack.amount += itemStack.amount;
            potion = newStack;
            return true;
        }

        if (inventory.TryGetValue(type, out _))
        {
            ItemStack newStack = inventory[type];
            newStack.amount += itemStack.amount;
            inventory[type] = newStack;
            return true;
        }

        if (inventory.Count < inventorySpace)
        {
            inventory[type] = itemStack;
            return true;
        }

        return false;
    }
}

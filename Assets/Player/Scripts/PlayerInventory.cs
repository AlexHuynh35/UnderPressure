using System;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public Weapon weaponInfo;
    [HideInInspector] public ItemStack weapon;
    [HideInInspector] public ItemStack arrow;
    [HideInInspector] public ItemStack potion;

    private EntityManager player;

    private void Awake()
    {
        player = GetComponent<EntityManager>();
        arrow = new ItemStack(arrowInfo, arrowAmount);
        weapon = new ItemStack(weaponInfo, 1);
    }
    
    void Start()
    {
        if (player.abilityManager is PlayerAbilityManager manager)
        {
            manager.SwitchWeapon(weaponInfo);
        }
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

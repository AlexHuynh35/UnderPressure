using System;
using UnityEngine;

public class ChestInventory : Inventory
{
    public bool opened;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager entity = other.gameObject.GetComponent<EntityManager>();
        if (entity != null && (entity.tags & EntityTag.Player) != 0 && !opened)
        {
            DropItem();
            opened = true;
        }
    }

    public void Initialize()
    {
        opened = false;
    }

    public override bool AddToInventory(ItemStack itemStack)
    {
        Type type = itemStack.item.GetType();

        inventory[type] = itemStack;
        return true;
    }
}

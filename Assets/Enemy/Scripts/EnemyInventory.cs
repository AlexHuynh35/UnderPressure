using UnityEngine;

public class EnemyInventory : Inventory
{
    void Start()
    {
        inventory[arrowInfo.GetType()] = new ItemStack(arrowInfo, arrowAmount);
    }

    void Update()
    {
        
    }

    public override void DropItem()
    {
        foreach (var itemStack in inventory)
        {
            ItemDrop drop = Instantiate(dropPrefab, AbilityHelper.OffsetLocation(transform.position, 1), Quaternion.identity).GetComponent<ItemDrop>();
            drop.Initialize(itemStack.Value);
        }
    }
}

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
}

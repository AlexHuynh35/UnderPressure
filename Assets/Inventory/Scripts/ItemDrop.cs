using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private ItemStack itemStack;

    public void Initialize(ItemStack itemStack)
    {
        this.itemStack = itemStack;
    }

    private void OnTriggerEnter(Collider other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (target != null && target.tags == EntityTag.Player)
        {
            target.inventory.AddToInventory(itemStack);
            Destroy(gameObject);
        }
    }
}

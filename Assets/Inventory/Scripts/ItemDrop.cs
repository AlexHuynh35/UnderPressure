using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private ItemStack itemStack;

    public void Initialize(ItemStack itemStack)
    {
        this.itemStack = itemStack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (target.tags == EntityTag.Player)
        {
            target.inventory.AddToInventory(itemStack);
            Destroy(gameObject);
        }
    }
}

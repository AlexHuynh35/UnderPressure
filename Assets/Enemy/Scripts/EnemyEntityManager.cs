using UnityEngine;

public class EnemyEntityManager : EntityManager
{
    protected override void OnDeath()
    {
        inventory.DropItem();
        Destroy(gameObject);
    }
}

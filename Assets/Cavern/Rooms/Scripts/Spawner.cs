using UnityEngine;

public class Spawner : MonoBehaviour
{
    private EnemyHolder enemyHolder;

    public void Initialize(EnemyHolder enemyHolder)
    {
        this.enemyHolder = enemyHolder;
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab, DropHolder dropHolder)
    {
        GameObject enemy = Instantiate(enemyPrefab, AbilityHelper.OffsetLocation(transform.position, 2), Quaternion.identity, enemyHolder.transform);
        enemy.GetComponent<EntityManager>().inventory.Initialize(dropHolder);
        return enemy;
    }
}

using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        return Instantiate(enemyPrefab, AbilityHelper.OffsetLocation(transform.position, 2), Quaternion.identity);
    }
}

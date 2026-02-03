using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RoomRules", menuName = "RoomRules")]
public class RoomRules : ScriptableObject
{
    [Header("Enemy Types")]
    public List<GameObject> enemyPrefabs;

    [Header("Rewards")]
    public List<Item> items;

    [Header("Rules")]
    public int rounds;
    public int averageEnemyPerRound;
    public int averageNumberRewardTypes;
    public int averageRewardAmountPerType;

    public GameObject GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    public int GetNumberEnemy()
    {
        return Random.Range(Mathf.Max(averageEnemyPerRound - 2, 1), averageEnemyPerRound + 2);
    }
}

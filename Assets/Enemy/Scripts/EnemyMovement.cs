using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private MovementPattern pattern;
    private EntityManager enemy;

    void Start()
    {
        enemy = GetComponent<EntityManager>();
    }

    void Update()
    {
        pattern.SetOrientation(enemy);
    }
}

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private EntityManager player;

    void Start()
    {
        player = GetComponent<EntityManager>();

        PlayerData.Set(player);
    }

    void Update()
    {
        
    }
}

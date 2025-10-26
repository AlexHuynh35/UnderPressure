using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private EntityManager player;

    void Start()
    {
        player = GetComponent<EntityManager>();
    }

    void Update()
    {
        Vector2 movementVector = GetMovementDirection();
        player.rb.linearVelocity = movementVector;
    }
    
    private Vector2 GetMovementDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movementDirection = player.transform.up * verticalInput + player.transform.right * horizontalInput;
        return movementDirection * player.speed * player.stunned;
    }
}

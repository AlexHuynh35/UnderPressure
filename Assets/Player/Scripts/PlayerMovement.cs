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

        if (Input.GetKeyDown(KeyCode.W))
            player.orientation = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S))
            player.orientation = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A))
            player.orientation = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D))
            player.orientation = Vector2.right;

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;
        return movementDirection * player.speed * player.stunned;
    }
}

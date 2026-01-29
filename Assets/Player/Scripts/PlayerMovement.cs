using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private EntityManager player;

    void Start()
    {
        player = GetComponent<EntityManager>();
    }

    void FixedUpdate()
    {
        if (!InputManager.Instance.receivingInputs) return;
        
        Vector2 movementVector = GetMovementDirection();
        player.rb.AddForce(movementVector, ForceMode2D.Force);
    }

    private Vector2 GetMovementDirection()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal") * player.confused, Input.GetAxisRaw("Vertical") * player.confused);

        if (input != Vector2.zero)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                player.orientation = input.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                player.orientation = input.y > 0 ? Vector2.up : Vector2.down;
            }
        }

        Vector2 movementDirection = input.normalized;
        return movementDirection * player.speed * player.stunned;
    }
}

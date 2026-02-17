using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private EntityManager player;
    private Rigidbody rb;

    void Start()
    {
        player = GetComponent<EntityManager>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!InputManager.Instance.receivingInputs) return;

        Vector2 movementDirection = GetMovementDirection();
        Vector3 movementVector = new Vector3(movementDirection.x, 0, movementDirection.y) * player.speed * player.stunned;
        rb.AddForce(movementVector, ForceMode.Force);
    }

    private Vector2 GetMovementDirection()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal") * player.confused, Input.GetAxisRaw("Vertical") * player.confused);

        if (input != Vector2.zero)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                player.ChangeOrientation(input.x > 0 ? Vector3.right : Vector3.left);
            }
            else
            {
                player.ChangeOrientation(input.y > 0 ? Vector3.up : Vector3.down);
            }
        }

        if (!player.rotate)
        {
            return input.normalized;
        }
        else
        {
            if (input != Vector2.zero)
            {
                float currentAngle = Mathf.Atan2(player.movementDirection.y, player.movementDirection.x) * Mathf.Rad2Deg;
                float targetAngle = Mathf.Atan2(input.normalized.y, input.normalized.x) * Mathf.Rad2Deg;
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, player.rotateSpeed * Time.fixedDeltaTime) * Mathf.Deg2Rad;
                player.ChangeMovementDirection(new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle)).normalized);
            }

            return player.movementDirection;
        }
    }
}

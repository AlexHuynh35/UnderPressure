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
        
        Vector3 movementVector = GetMovementDirection() * player.speed * player.stunned;
        player.rb.AddForce(movementVector, ForceMode.Force);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal") * player.confused, 0, Input.GetAxisRaw("Vertical") * player.confused);

        if (input != Vector3.zero)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.z))
            {
                player.ChangeOrientation(input.x > 0 ? Vector3.right : Vector3.left);
            }
            else
            {
                player.ChangeOrientation(input.z > 0 ? Vector3.forward : Vector3.back);
            }
        }

        if (!player.rotate)
        {
            return input.normalized;
        }
        else
        {
            if (input != Vector3.zero)
            {
                float currentAngle = Mathf.Atan2(player.movementDirection.z, player.movementDirection.x) * Mathf.Rad2Deg;
                float targetAngle = Mathf.Atan2(input.normalized.z, input.normalized.x) * Mathf.Rad2Deg;
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, player.rotateSpeed * Time.fixedDeltaTime) * Mathf.Deg2Rad;
                player.ChangeMovementDirection(new Vector3(Mathf.Cos(newAngle), 0, Mathf.Sin(newAngle)).normalized);
            }

            return player.movementDirection;
        }
    }
}

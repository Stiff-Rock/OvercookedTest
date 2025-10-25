using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float rotationSpeed = 50;

    [SerializeField] private Key forwardKey;
    [SerializeField] private Key leftKey;
    [SerializeField] private Key backwardKey;
    [SerializeField] private Key rightKey;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current[forwardKey].isPressed)
            moveDirection += Vector3.forward;

        if (Keyboard.current[leftKey].isPressed)
            moveDirection += -Vector3.right;

        if (Keyboard.current[backwardKey].isPressed)
            moveDirection += -Vector3.forward;

        if (Keyboard.current[rightKey].isPressed)
            moveDirection += Vector3.right;

        // Move the player
        Vector3 moveDirectionNormalized = moveDirection.normalized;
        transform.position += moveDirectionNormalized * movementSpeed * Time.deltaTime;

        if (moveDirection == Vector3.zero) return;

        // Rotate to movement direction
        Quaternion rotationTarget = Quaternion.LookRotation(moveDirectionNormalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);
    }
}
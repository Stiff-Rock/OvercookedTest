using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Animator animator;
    private static readonly int isWalkingHash = Animator.StringToHash("IsWalking");

    // Values
    [Header("VALUES")]
    private Vector3 moveDirection;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float rotationSpeed = 50;
    private bool isWalking;

    // Controls
    [Header("CONTROLS")]
    [SerializeField] private Key forwardKey;
    [SerializeField] private Key leftKey;
    [SerializeField] private Key backwardKey;
    [SerializeField] private Key rightKey;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GatherInputs();
        Move();
    }

    private void GatherInputs()
    {
        moveDirection = Vector3.zero;

        if (Keyboard.current[forwardKey].isPressed)
            moveDirection += Vector3.forward;

        if (Keyboard.current[leftKey].isPressed)
            moveDirection += -Vector3.right;

        if (Keyboard.current[backwardKey].isPressed)
            moveDirection += -Vector3.forward;

        if (Keyboard.current[rightKey].isPressed)
            moveDirection += Vector3.right;
    }

    private void Move()
    {
        bool isCurrentlyMoving = moveDirection != Vector3.zero;
        if (isWalking != isCurrentlyMoving)
        {
            isWalking = isCurrentlyMoving;
            animator.SetBool(isWalkingHash, isWalking);
        }

        if (!isWalking) return;

        // Move the player
        Vector3 moveDirectionNormalized = moveDirection.normalized;
        transform.position += movementSpeed * Time.deltaTime * moveDirectionNormalized;

        // Rotate to movement direction
        Quaternion rotationTarget = Quaternion.LookRotation(moveDirectionNormalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);
    }
}
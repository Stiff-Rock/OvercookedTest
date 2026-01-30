using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // References
    private CharacterController characterController;
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
        characterController = GetComponent<CharacterController>();
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

        Vector3 moveDirectionNormalized = moveDirection.normalized;

        // Rotate to movement direction
        Quaternion rotationTarget = Quaternion.LookRotation(moveDirectionNormalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);

        // Move the player
        characterController.SimpleMove(moveDirectionNormalized * movementSpeed);
    }
}
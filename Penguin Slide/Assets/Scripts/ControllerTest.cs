using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleTileMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private PlayerInput input;
    private InputAction moveAction;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector2 lastMoveInput = Vector2.zero;

    void Awake()
    {
        input = new PlayerInput();
        moveAction = input.Player.Move;
        animator = GetComponent<Animator>();

        targetPosition = transform.position;

        // Initialize animator to idle
        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", -1f);
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsPushing", false);
    }

    void OnEnable()
    {
        input.Enable();
        moveAction.Enable();
    }

    void OnDisable()
    {
        input.Disable();
        moveAction.Disable();
    }

    void Update()
    {
        // Only accept new input if we're not currently moving
        if (!isMoving)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            // Only move in cardinal directions (up, down, left, right)
            if (moveInput.x != 0 || moveInput.y != 0)
            {
                // Prioritize horizontal movement
                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                {
                    moveInput.y = 0;
                    moveInput.x = moveInput.x > 0 ? 1 : -1;
                }
                else
                {
                    moveInput.x = 0;
                    moveInput.y = moveInput.y > 0 ? 1 : -1;
                }

                // Store the move direction
                lastMoveInput = moveInput;

                // Set new target position (1 tile = 1 unit)
                targetPosition = transform.position + new Vector3(moveInput.x, moveInput.y, 0f);
                isMoving = true;

                Debug.Log($"Moving to: {targetPosition}");
            }
        }

        // Move towards target
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if we've reached the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
                Debug.Log("Reached destination");
            }
        }

        // Update animator
        animator.SetFloat("MoveX", lastMoveInput.x);
        animator.SetFloat("MoveY", lastMoveInput.y);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsPushing", false);
    }
}
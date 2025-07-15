using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Camera Reference")]
    public Transform cameraTransform;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Double Jump Settings")]
    public int maxJumps = 2;
    public float doubleJumpSpeedBoost = 1.2f;

    [HideInInspector]
    public Vector3 currentMoveVelocity; // ← used by HeadBob or camera scripts
    [HideInInspector]
    public bool isGrounded;

    private CharacterController controller;
    private Vector3 velocity;
    private int jumpCount = 0;
    private bool usedDoubleJump = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
            usedDoubleJump = false;
        }

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (usedDoubleJump)
            currentSpeed *= doubleJumpSpeedBoost;

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 horizontalMove = move * currentSpeed;

        // Save for HeadBob use
        currentMoveVelocity = horizontalMove;

        controller.Move(horizontalMove * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpCount == 2)
                usedDoubleJump = true;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

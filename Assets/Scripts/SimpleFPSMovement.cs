using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSMovement : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;

    public float groundCheckDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Double Jump Settings")]
    public int maxJumps = 2;
    public float doubleJumpSpeedBoost = 1.2f; // Optional speed boost on double jump

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

        // If double jump is active, increase movement speed slightly
        if (usedDoubleJump)
            currentSpeed *= doubleJumpSpeedBoost;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpCount == 2)
                usedDoubleJump = true;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

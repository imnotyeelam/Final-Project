using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    [Header("Jump Physics")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Camera Reference")]
    public Transform cameraTransform;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Double Jump Settings")]
    public int maxJumps = 2;
    public float doubleJumpSpeedBoost = 1.2f;

    [Header("Sprint Settings")]
    public float maxSprintTime = 5f;
    public int maxEnergy = 20;
    public int sprintEnergyCost = 5;

    [HideInInspector] public Vector3 currentMoveVelocity;
    [HideInInspector] public bool isGrounded;

    private CharacterController controller;
    private Vector3 velocity;
    private int jumpCount = 0;
    private bool usedDoubleJump = false;

    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private int currentEnergy;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentEnergy = maxEnergy;
    }

    [System.Obsolete]
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

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = walkSpeed;

        // Sprinting logic
        bool sprintKey = Input.GetKey(KeyCode.LeftShift);
        bool canSprint = isGrounded && sprintKey && z > 0 && currentEnergy >= sprintEnergyCost;

        if (canSprint && !isSprinting)
        {
            isSprinting = true;
            sprintTimer = maxSprintTime;
            currentEnergy -= sprintEnergyCost;
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;

            if (sprintTimer > 0f)
            {
                currentSpeed = runSpeed;
            }
            else
            {
                isSprinting = false;
                HandSwitcher.CurrentMode = 0;

                HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
                if (handSwitcher != null)
                {
                    handSwitcher.SetHandMode(0);
                }
            }
        }

        Vector3 horizontalMove = move * currentSpeed;
        currentMoveVelocity = horizontalMove;
        controller.Move(horizontalMove * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpCount == 2)
                usedDoubleJump = true;
        }

        // Gravity
        if (velocity.y < 0)
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

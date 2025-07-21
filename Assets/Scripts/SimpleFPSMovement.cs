using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSMovement : MonoBehaviour
{
    public CharacterController charCon;
    public static SimpleFPSMovement instance;
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
    public float maxSprintTime = 10f;
    public int maxEnergy = 20;
    public int sprintEnergyCost = 5;

    [Header("Audio")]
    public AudioClip walkClip;
    public AudioClip sprintClip;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public float stepRate = 0.5f;

    private AudioSource audioSource;
    private float stepTimer;
    private PlayerVitalsManager vitalsManager;

    [HideInInspector] public Vector3 currentMoveVelocity;
    [HideInInspector] public bool isGrounded;

    private CharacterController controller;
    private Vector3 velocity;
    private int jumpCount = 0;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private int currentEnergy;
    private bool wasGroundedLastFrame;
    public bool canMove = true; 

    [System.Obsolete]
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        currentEnergy = maxEnergy;

        vitalsManager = FindObjectOfType<PlayerVitalsManager>();
        if (vitalsManager == null)
        {
            Debug.LogError("❌ PlayerVitalsManager not found!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [System.Obsolete]
    void Update()
    {
        if (!canMove) return;
        // Use CharacterController's built-in ground check
        isGrounded = controller.isGrounded;

        // Predict landing 0.2 seconds earlier using raycast
        if (!wasGroundedLastFrame && !isGrounded && velocity.y < -1f)
        {
            RaycastHit hit;
            float predictionDistance = 0.6f; // Adjust for anticipation

            if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, predictionDistance, groundMask))
            {
                if (landClip != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(landClip);
                }
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0; // ✅ Reset jump count when grounded on ANY surface
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = walkSpeed;

        // Sprinting
        bool sprintKey = Input.GetKey(KeyCode.LeftShift);
        bool tryToSprint = isGrounded && sprintKey && z > 0;

        if (tryToSprint && !isSprinting)
        {
            if (vitalsManager != null && vitalsManager.currentEnergy >= 5)
            {
                vitalsManager.ConsumeEnergy(5f);
                Debug.Log("[Sprint] Used 5 energy. Remaining: " + vitalsManager.currentEnergy);

                isSprinting = true;
                sprintTimer = maxSprintTime;
            }
            else
            {
                Debug.Log("[Sprint] Not enough energy to sprint!");
            }
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

                HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
                if (handSwitcher != null)
                    handSwitcher.SetHandMode(0);
            }
        }

        // Move player
        Vector3 horizontalMove = move * currentSpeed;
        currentMoveVelocity = horizontalMove;
        controller.Move(horizontalMove * Time.deltaTime);

        // Footsteps
        if (move.magnitude > 0.1f && isGrounded)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (isSprinting && sprintClip != null)
                    audioSource.PlayOneShot(sprintClip);
                else if (walkClip != null)
                    audioSource.PlayOneShot(walkClip);

                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpClip != null)
                audioSource.PlayOneShot(jumpClip);
        }

        // Gravity
        if (velocity.y < 0)
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        wasGroundedLastFrame = isGrounded;
    }
}

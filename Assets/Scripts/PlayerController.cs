using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    private bool isClimbing = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Optional: lock the mouse
    }

    void Update()
    {
        GroundCheck();
        Move();
        HandleJump();
        ApplyGravity();
        Climb();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevent gravity stacking
            jumpCount = 0;    // Reset jump count on landing
        }
    }

    void Move()
    {
        if (isClimbing) return; // Disable normal move when climbing

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (isClimbing) return; // Don't jump when climbing

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < 1) // Allow double jump
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
            }
        }
    }

    void ApplyGravity()
    {
        if (!isClimbing)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void Climb()
    {
        if (isClimbing)
        {
            float v = Input.GetAxis("Vertical");
            Vector3 climbDir = new Vector3(0, v, 0);
            controller.Move(climbDir * climbSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            velocity.y = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}

using UnityEngine;

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

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && !isClimbing)
        {
            if (jumpCount > 0)
                jumpCount = 0;

            if (velocity.y < 0)
                velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;

        if (isClimbing)
        {
            float v = Input.GetAxis("Vertical");
            move = new Vector3(0f, v * climbSpeed, 0f);
            velocity = Vector3.zero;
        }
        else
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 horizontal = transform.right * x + transform.forward * z;
            move = horizontal * speed;

            velocity.y += gravity * Time.deltaTime;
            move.y = velocity.y;
        }

        HandleJump();
        controller.Move(move * Time.deltaTime);
    }

    void HandleJump()
    {
        if (isClimbing) return;

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < 1)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            velocity = Vector3.zero;
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

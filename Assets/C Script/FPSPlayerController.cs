using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 30f;
    public Transform playerCamera;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Optional: Hide cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotate view (Mouse)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Jump input (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        // Move player (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * moveSpeed;
        velocity.y = rb.velocity.y; // Preserve vertical motion (jump/fall)

        rb.velocity = velocity; // ✅ USE THIS (not .linearVelocity)
    }

    void OnCollisionStay(Collision collision)
    {
        // Assume grounded if touching anything
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance; // ← 加上这一行！

    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float gravityModifier = 2.5f;
    public float jumpPower = 8f;
    public float mouseSensitivity = 2f;

    private Vector3 moveInput;
    private int jumpCount = 0;
    private float verticalRotation = 0f;

    private CharacterController charCon;
    public Transform camTrans;

    void Awake()
    {
        instance = this; // ← 再加上这一行！
    }

    void Start()
    {
        charCon = GetComponent<CharacterController>();

        if (camTrans == null && Camera.main != null)
            camTrans = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float yStore = moveInput.y;

        Vector3 forward = transform.forward * Input.GetAxis("Vertical");
        Vector3 right = transform.right * Input.GetAxis("Horizontal");

        moveInput = (forward + right).normalized;

        moveInput *= Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        moveInput.y = yStore;

        // Apply gravity
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        // Ground check and jump
        if (charCon.isGrounded)
        {
            jumpCount = 0;
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            moveInput.y = jumpPower;
            jumpCount++;
        }

        charCon.Move(moveInput * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera (vertical)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -85f, 85f); // prevent flipping over
        camTrans.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}

using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Walking Settings")]
    public float walkFrequency = 1.6f;
    public float walkHorizontalAmplitude = 0.05f;
    public float walkVerticalAmplitude = 0.03f;

    [Header("Sprinting Settings")]
    public float sprintFrequency = 2.5f;
    public float sprintHorizontalAmplitude = 0.1f;
    public float sprintVerticalAmplitude = 0.08f;
    public float sprintForwardTilt = 0.05f;

    [Header("Strafe Tilt Settings")]
    public float strafeTiltAngle = 5f;          // degrees of tilt
    public float tiltSpeed = 5f;                // how fast it rotates

    public SimpleFPSMovement playerMovement;

    private float timer = 0f;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        if (playerMovement == null)
        {
            Debug.LogError("HeadBob: Missing playerMovement reference!");
        }
    }

    void Update()
    {
        if (playerMovement == null) return;

        Vector3 horizontalVelocity = playerMovement.currentMoveVelocity;
        float speed = horizontalVelocity.magnitude;

        bool isMoving = speed > 0.1f && playerMovement.isGrounded;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        bool isStrafingLeft = Input.GetKey(KeyCode.A);
        bool isStrafingRight = Input.GetKey(KeyCode.D);

        // Determine movement bobbing
        if (isMoving)
        {
            float frequency = isSprinting ? sprintFrequency : walkFrequency;
            float ampX = isSprinting ? sprintHorizontalAmplitude : walkHorizontalAmplitude;
            float ampY = isSprinting ? sprintVerticalAmplitude : walkVerticalAmplitude;
            float forwardOffset = isSprinting ? -sprintForwardTilt : 0f;

            timer += Time.deltaTime * frequency;

            float bobX = Mathf.Sin(timer) * ampX;
            float bobY = Mathf.Abs(Mathf.Cos(timer * 2f)) * ampY;

            Vector3 bobOffset = new Vector3(bobX, bobY, forwardOffset);
            transform.localPosition = startLocalPosition + bobOffset;
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPosition, Time.deltaTime * 6f);
        }

        // Handle strafing camera tilt (roll)
        float targetTilt = 0f;
        if (isStrafingLeft) targetTilt = strafeTiltAngle;
        else if (isStrafingRight) targetTilt = -strafeTiltAngle;

        Quaternion targetRotation = Quaternion.Euler(startLocalRotation.eulerAngles.x, startLocalRotation.eulerAngles.y, targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
    }
}

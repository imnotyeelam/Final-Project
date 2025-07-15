using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Walking Settings")]
    public float bobFrequency = 1.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;

    [Header("Running Settings")]
    public float runBobFrequency = 2.5f;
    public float runBobHorizontalAmplitude = 0.08f;
    public float runBobVerticalAmplitude = 0.1f;

    public SimpleFPSMovement playerMovement;

    private float timer = 0f;
    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = transform.localPosition;

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
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        if (isMoving)
        {
            float frequency = isRunning ? runBobFrequency : bobFrequency;
            float amplitudeX = isRunning ? runBobHorizontalAmplitude : bobHorizontalAmplitude;
            float amplitudeY = isRunning ? runBobVerticalAmplitude : bobVerticalAmplitude;

            timer += Time.deltaTime * frequency;

            float bobX = Mathf.Sin(timer) * amplitudeX;
            float bobY = Mathf.Cos(timer * 2f) * amplitudeY;

            transform.localPosition = startLocalPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPosition, Time.deltaTime * 5f);
        }
    }
}

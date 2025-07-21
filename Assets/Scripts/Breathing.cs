using UnityEngine;

public class Breathing : MonoBehaviour
{
    [Header("Idle Settings")]
    public float idleBreathSpeed = 1.5f;
    public float idleBreathAmount = 0.01f;

    [Header("Running Settings")]
    public float runBreathSpeed = 3f;
    public float runBreathAmount = 0.03f;

    private Vector3 originalPosition;
    private float timer = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputZ) > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        if (isRunning)
        {
            // Running breathing (faster and bigger)
            timer += Time.deltaTime * runBreathSpeed;
            float offsetY = Mathf.Sin(timer) * runBreathAmount;
            transform.localPosition = originalPosition + new Vector3(0, offsetY, 0);
        }
        else if (!isMoving)
        {
            // Idle breathing
            timer += Time.deltaTime * idleBreathSpeed;
            float offsetY = Mathf.Sin(timer) * idleBreathAmount;
            transform.localPosition = originalPosition + new Vector3(0, offsetY, 0);
        }
        else
        {
            // Reset position when walking (no breathing while walking)
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
            timer = 0f;
        }
    }
}

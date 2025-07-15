using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobFrequency = 1.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;
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

        if (speed > 0.1f && playerMovement.isGrounded)
        {
            timer += Time.deltaTime * bobFrequency;

            float bobX = Mathf.Sin(timer) * bobHorizontalAmplitude;
            float bobY = Mathf.Cos(timer * 2f) * bobVerticalAmplitude;

            transform.localPosition = startLocalPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            // Reset to base position when not moving
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPosition, Time.deltaTime * 5f);
        }
    }
}

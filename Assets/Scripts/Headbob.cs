using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobFrequency = 1.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;
    public CharacterController playerController;

    private float timer = 0f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if (playerController == null) return;

        if (playerController.velocity.magnitude > 0.1f && playerController.isGrounded)
        {
            timer += Time.deltaTime * bobFrequency;
            float bobX = Mathf.Sin(timer) * bobHorizontalAmplitude;
            float bobY = Mathf.Cos(timer * 2) * bobVerticalAmplitude;
            transform.localPosition = startPosition + new Vector3(bobX, bobY, 0);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * 5f);
        }
    }
}

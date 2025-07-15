using UnityEngine;

public class IdleBreathing : MonoBehaviour
{
    public float breathSpeed = 1.5f;
    public float breathAmount = 0.01f;

    private Vector3 originalPosition;
    private float timer = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Only breathe when standing still
        bool isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        if (!isMoving)
        {
            timer += Time.deltaTime * breathSpeed;
            float offsetY = Mathf.Sin(timer) * breathAmount;
            transform.localPosition = originalPosition + new Vector3(0, offsetY, 0);
        }
        else
        {
            // Reset position while walking
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
            timer = 0f;
        }
    }
}

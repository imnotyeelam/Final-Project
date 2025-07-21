using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    public float tiltAmount = 4f;
    public float tiltSpeed = 5f;

    private float currentTilt = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float targetTilt = -mouseX * tiltAmount;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}

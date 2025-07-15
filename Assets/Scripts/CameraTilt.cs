using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    public float tiltAmount = 5f;
    public float tiltSpeed = 5f;
    public CharacterController controller;

    private float currentTilt = 0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float targetTilt = horizontalInput * -tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        transform.localRotation = Quaternion.Euler(0, 0, currentTilt);
    }
}

using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Transform hipPosition;      // Default position
    public Transform aimPosition;      // Aiming position
    public float aimSpeed = 10f;

    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float fovTransitionSpeed = 5f;

    private bool isAiming = false;

    void Update()
    {
        isAiming = Input.GetMouseButton(1); // Right mouse button

        // Move gun to aiming position
        if (isAiming)
        {
            transform.position = Vector3.Lerp(transform.position, aimPosition.position, Time.deltaTime * aimSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, aimPosition.rotation, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, hipPosition.position, Time.deltaTime * aimSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, hipPosition.rotation, Time.deltaTime * aimSpeed);
        }

        // Camera FOV change
        if (playerCamera != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        }
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}

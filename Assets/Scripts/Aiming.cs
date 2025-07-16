using UnityEngine;

public class Aiming : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float gunAimFOV = 40f;
    public float hookAimFOV = 50f; // Different FOV for hook aiming
    public float fovTransitionSpeed = 5f;

    [Header("Camera Positions")]
    public Transform normalCameraPos;
    public Transform gunAimCameraPos;
    public Transform hookAimCameraPos; // New hook aim position
    public float cameraMoveSpeed = 5f;

    void Update()
    {
        if (playerCamera == null) return;

        // Handle FOV changes
        float targetFOV = normalFOV;
        Transform targetPos = normalCameraPos;

        if (HandSwitcher.IsAiming)
        {
            if (HandSwitcher.CurrentMode == 1) // Hook aim
            {
                targetFOV = hookAimFOV;
                targetPos = hookAimCameraPos;
            }
            else if (HandSwitcher.CurrentMode == 3) // Gun aim
            {
                targetFOV = gunAimFOV;
                targetPos = gunAimCameraPos;
            }
        }

        // Smooth transitions
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView, 
            targetFOV, 
            Time.deltaTime * fovTransitionSpeed
        );

        playerCamera.transform.position = Vector3.Lerp(
            playerCamera.transform.position,
            targetPos.position,
            Time.deltaTime * cameraMoveSpeed
        );

        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation,
            targetPos.rotation,
            Time.deltaTime * cameraMoveSpeed
        );
    }
}
using UnityEngine;

public class CameraAimer : MonoBehaviour
{
    public Camera playerCamera;

    [Header("Field of View Settings")]
    public float normalFOV = 60f;
    public float gunAimFOV = 30f;
    public float hookAimFOV = 35f;
    public float transitionSpeed = 5f;

    void Update()
    {
        if (playerCamera == null) return;

        float targetFOV = normalFOV;

        // Check aiming state
        if (HandSwitcher.IsAiming)
        {
            if (HandSwitcher.CurrentMode == 3) // Gun mode
            {
                targetFOV = gunAimFOV;
            }
            else if (HandSwitcher.CurrentMode == 1) // Hook mode
            {
                targetFOV = hookAimFOV;
            }
        }

        // Smooth zoom transition
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * transitionSpeed
        );
    }
}

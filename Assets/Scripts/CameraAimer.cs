using UnityEngine;

public class CameraAimer : MonoBehaviour
{
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float gunAimFOV = 30f;
    public float hookAimFOV = 35f;
    public float transitionSpeed = 5f;

    void Update()
    {
        if (playerCamera == null) return;

        float targetFOV = normalFOV;

        if (HandSwitcher.IsAiming)
        {
            if (HandSwitcher.CurrentMode == HandSwitcher.Mode.Gun) targetFOV = gunAimFOV;
            else if (HandSwitcher.CurrentMode == HandSwitcher.Mode.Hook) targetFOV = hookAimFOV;
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * transitionSpeed);
    }
}

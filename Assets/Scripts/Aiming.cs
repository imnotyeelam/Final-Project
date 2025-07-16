using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Transform hipPosition;
    public Transform aimPosition;
    public float aimSpeed = 10f;

    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float fovSpeed = 5f;

    private bool isAiming;

    void Update()
    {
        isAiming = Input.GetMouseButton(1); // Right click

        // Move gun between hip and aim positions
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

        // FOV change
        if (playerCamera)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovSpeed);
        }
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}

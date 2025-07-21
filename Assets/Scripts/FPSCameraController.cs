using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    //可以换成playercontroller一起集成
    public Transform cameraHolder; // Drag in CameraHolder
    public float mouseSensitivity = 2f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 去掉鼠标——这个挂到GameManager里面
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player (left/right)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera (up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
using UnityEngine;

public class L3PlayerController : MonoBehaviour
{
    public static L3PlayerController instance;

    public float moveSpeed = 5f;
    public CharacterController charCon;

    public float mouseSensitivity = 2f;
    private float verticalLookRotation = 0f;

    [Header("Camera Target")]
    public Transform CameraTarget; // 绑定 Player 头部的空物体

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // 移动
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * inputX + transform.forward * inputZ;
        charCon.Move(moveDir * moveSpeed * Time.deltaTime);

        // 处理鼠标输入
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // 左右旋转（控制 Player）
        transform.Rotate(Vector3.up * mouseX);

        // 垂直视角（控制虚拟相机的 LookAt 目标）
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f); // 限制角度

        // 调整 CameraTarget 的角度
        // 你的 CameraTarget 需要拖到 inspector
        CameraTarget.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

}

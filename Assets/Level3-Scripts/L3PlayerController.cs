using UnityEngine;

public class L3PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CharacterController charCon;
    public Transform cameraTransform; //拖入你的主相机

    public float mouseSensitivity;

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // 获取摄像机朝向的 forward 和 right，但去掉垂直分量
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 基于摄像机的方向来移动
        Vector3 moveDir = camRight * inputX + camForward * inputZ;

        charCon.Move(moveDir * moveSpeed * Time.deltaTime);

        //camera rotation control look left&right
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x - mouseInput.y, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
    }
}

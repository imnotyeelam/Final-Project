using UnityEngine;

public class L3PlaneController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float strafeSpeed = 10f;
    public float liftSpeed = 8f;
    public float rotationSpeed = 90f;
    public float maxSpeed = 30f;

    private Transform playerTransform;
    private bool isActive = false;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (!isActive) return;

        HandleRotation();
        HandleMovement();

        // 同步玩家位置
        if (playerTransform != null)
        {
            playerTransform.position = transform.position;
        }

        // 下飞机
        if (Input.GetKeyDown(KeyCode.E))
        {
            DeactivatePlane();
        }
    }

    void HandleMovement()
    {
        float forward = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");
        float lift = 0;

        if (Input.GetKey(KeyCode.Space)) lift = 1;
        if (Input.GetKey(KeyCode.LeftControl)) lift = -1;

        Vector3 moveDir = (transform.forward * forward * moveSpeed) +
                          (transform.right * strafe * strafeSpeed) +
                          (transform.up * lift * liftSpeed);

        transform.position += moveDir * Time.deltaTime;
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right * -mouseY * rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void ActivatePlane(Transform player)
    {
        isActive = true;
        playerTransform = player;
        Debug.Log("飞机模式启动");
    }

    public void DeactivatePlane()
    {
        isActive = false;
        PlayerController1.instance.ExitPlane();
        Debug.Log("退出飞机模式");
    }
}

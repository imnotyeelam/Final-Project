using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Quaternion originalLocalRotation;
    private Transform currentPlatform;
    private CharacterController characterController;

    // 添加平滑过渡参数
    [SerializeField] private float rotationTransitionSpeed = 5f;
    private bool isTransitioning = false;
    private Quaternion targetRotation;


    void Start()
    {
        originalParent = transform.parent;
        originalLocalRotation = transform.localRotation;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckPlatformBelow();

        // 处理旋转过渡
        if (isTransitioning)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                rotationTransitionSpeed * Time.deltaTime);

            // 检查是否接近目标旋转
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                transform.rotation = targetRotation;
                isTransitioning = false;
            }
        }
    }

    void CheckPlatformBelow()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, characterController.height / 2 + 0.2f))
        {
            if (hit.collider.CompareTag("MovingPlatform"))
            {
                if (currentPlatform != hit.collider.transform)
                {
                    transform.parent = hit.collider.transform;
                    currentPlatform = hit.collider.transform;
                    isTransitioning = false; // 停止任何进行中的过渡
                }
            }
            else
            {
                ResetToOriginalParent();
            }
        }
        else
        {
            ResetToOriginalParent();
        }
    }

    void ResetToOriginalParent()
    {
        if (currentPlatform != null)
        {
            // 获取当前世界旋转
            Quaternion currentWorldRotation = transform.rotation;

            // 分离出Y轴旋转（水平旋转）
            float yRotation = currentWorldRotation.eulerAngles.y;

            // 重置父对象
            transform.parent = originalParent;

            // 创建只保留Y轴旋转的目标旋转
            // 保留当前Y轴旋转，但X和Z轴使用原始值
            Vector3 originalEuler = originalLocalRotation.eulerAngles;
            targetRotation = Quaternion.Euler(originalEuler.x, yRotation, originalEuler.z);

            // 开始平滑过渡到目标旋转
            isTransitioning = true;

            currentPlatform = null;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.collider.transform)
            {
                transform.parent = hit.collider.transform;
                currentPlatform = hit.collider.transform;
                isTransitioning = false; // 停止任何进行中的过渡
            }
        }
    }
}
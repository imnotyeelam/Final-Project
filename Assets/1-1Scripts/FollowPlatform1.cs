using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    // 获取真正需要移动的根物体（人物）
    private Transform playerRoot;

    void Start()
    {
        // 找到顶层的"人物"父物体
        playerRoot = transform.parent; // "人物"
        originalParent = playerRoot.parent; // "人物"的父级
        originalRotation = playerRoot.rotation;
        originalPosition = playerRoot.position;

        Debug.Log($"Player根物体: {playerRoot.name}");
        Debug.Log($"原始父级: {(originalParent ? originalParent.name : "null")}");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform") && currentPlatform == null)
        {
            Debug.Log($"到平台上了 - 平台: {hit.collider.name}");
            Debug.Log($"设置前 - PlayerRoot父级: {(playerRoot.parent ? playerRoot.parent.name : "null")}");

            // 设置"人物"的父级为移动平台
            playerRoot.parent = hit.collider.transform;
            currentPlatform = hit.collider.transform;

            Debug.Log($"设置后 - PlayerRoot父级: {(playerRoot.parent ? playerRoot.parent.name : "null")}");
        }
    }

    void Update()
    {
        if (currentPlatform != null)
        {
            // 从Player位置发射射线检测
            Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * 2.0f, Color.red, 0.1f);

            if (Physics.Raycast(ray, out RaycastHit hit, 2.0f))
            {
                // 检查射线是否击中当前平台或其子物体
                Transform hitTransform = hit.collider.transform;
                bool stillOnPlatform = (hitTransform == currentPlatform) ||
                                     hitTransform.IsChildOf(currentPlatform) ||
                                     currentPlatform.IsChildOf(hitTransform);

                if (!stillOnPlatform)
                {
                    Debug.Log($"射线击中其他物体: {hit.collider.name}，离开平台");
                    DetachFromPlatform();
                }
            }
            else
            {
                Debug.Log("射线未检测到地面，离开平台");
                DetachFromPlatform();
            }
        }
    }

    void DetachFromPlatform()
    {
        if (playerRoot == null) return;

        Debug.Log("开始脱离平台");

        // 暂存当前世界位置
        Vector3 currentPosition = playerRoot.position;

        // 脱离父物体
        playerRoot.parent = originalParent;

        // 恢复到水平旋转（只保留Y轴旋转，清除X和Z轴的倾斜）
        Vector3 eulerAngles = playerRoot.eulerAngles;
        playerRoot.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

        // 保持当前位置
        playerRoot.position = currentPosition;

        currentPlatform = null;
        Debug.Log($"脱离平台完成 - PlayerRoot父级: {(playerRoot.parent ? playerRoot.parent.name : "null")}");
    }
}
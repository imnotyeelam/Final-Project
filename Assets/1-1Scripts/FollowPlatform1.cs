using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;
    private Quaternion originalRotation;
    private Vector3 originalPosition; // 保存原始位置作为参考

    void Start()
    {
        originalParent = transform.parent;
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform") && currentPlatform == null)
        {
            // 直接设置父物体，让玩家跟随平台移动和旋转
            transform.parent = hit.collider.transform;
            currentPlatform = hit.collider.transform;
        }
    }

    void Update()
    {
        if (currentPlatform != null)
        {
            // 射线检测脚下是否还有平台
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
            {
                if (hit.collider.transform != currentPlatform)
                {
                    // 离开平台
                    DetachFromPlatform();
                }
            }
            else
            {
                // 没有检测到地面，说明已经跳开或掉落
                DetachFromPlatform();
            }
        }
    }

    void DetachFromPlatform()
    {
        // 暂存当前世界位置
        Vector3 currentPosition = transform.position;

        // 脱离父物体
        transform.parent = originalParent;

        // 恢复到水平旋转（只保留Y轴旋转，清除X和Z轴的倾斜）
        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

        // 保持当前位置
        transform.position = currentPosition;

        currentPlatform = null;
    }
}
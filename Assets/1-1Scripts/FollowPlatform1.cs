using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;

    void Start()
    {
        originalParent = transform.parent;

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            transform.parent = hit.collider.transform; // 跟随平台
            currentPlatform = hit.collider.transform;
        }
    }

    void Update()
    {
        // 检查是否不再站在平台上
        if (currentPlatform != null)
        {
            // 射线检测脚下是否还有平台
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
            {
                if (hit.collider.transform != currentPlatform)
                {
                    // 离开平台
                    transform.parent = originalParent;
                    currentPlatform = null;
                }
            }
            else
            {
                // 没有检测到地面，说明已经跳开或掉落
                transform.parent = originalParent;
                currentPlatform = null;
            }
        }
    }
}
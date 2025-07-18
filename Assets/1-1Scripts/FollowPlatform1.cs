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
            transform.parent = hit.collider.transform; // ����ƽ̨
            currentPlatform = hit.collider.transform;
        }
    }

    void Update()
    {
        // ����Ƿ���վ��ƽ̨��
        if (currentPlatform != null)
        {
            // ���߼������Ƿ���ƽ̨
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
            {
                if (hit.collider.transform != currentPlatform)
                {
                    // �뿪ƽ̨
                    transform.parent = originalParent;
                    currentPlatform = null;
                }
            }
            else
            {
                // û�м�⵽���棬˵���Ѿ����������
                transform.parent = originalParent;
                currentPlatform = null;
            }
        }
    }
}
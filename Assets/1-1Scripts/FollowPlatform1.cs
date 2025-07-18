using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;
    private Quaternion originalRotation;
    private Vector3 originalPosition; // ����ԭʼλ����Ϊ�ο�

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
            // ֱ�����ø����壬����Ҹ���ƽ̨�ƶ�����ת
            transform.parent = hit.collider.transform;
            currentPlatform = hit.collider.transform;
        }
    }

    void Update()
    {
        if (currentPlatform != null)
        {
            // ���߼������Ƿ���ƽ̨
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
            {
                if (hit.collider.transform != currentPlatform)
                {
                    // �뿪ƽ̨
                    DetachFromPlatform();
                }
            }
            else
            {
                // û�м�⵽���棬˵���Ѿ����������
                DetachFromPlatform();
            }
        }
    }

    void DetachFromPlatform()
    {
        // �ݴ浱ǰ����λ��
        Vector3 currentPosition = transform.position;

        // ���븸����
        transform.parent = originalParent;

        // �ָ���ˮƽ��ת��ֻ����Y����ת�����X��Z�����б��
        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

        // ���ֵ�ǰλ��
        transform.position = currentPosition;

        currentPlatform = null;
    }
}
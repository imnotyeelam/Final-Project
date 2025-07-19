using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    // ��ȡ������Ҫ�ƶ��ĸ����壨���
    private Transform playerRoot;

    void Start()
    {
        // �ҵ������"����"������
        playerRoot = transform.parent; // "����"
        originalParent = playerRoot.parent; // "����"�ĸ���
        originalRotation = playerRoot.rotation;
        originalPosition = playerRoot.position;

        Debug.Log($"Player������: {playerRoot.name}");
        Debug.Log($"ԭʼ����: {(originalParent ? originalParent.name : "null")}");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform") && currentPlatform == null)
        {
            Debug.Log($"��ƽ̨���� - ƽ̨: {hit.collider.name}");
            Debug.Log($"����ǰ - PlayerRoot����: {(playerRoot.parent ? playerRoot.parent.name : "null")}");

            // ����"����"�ĸ���Ϊ�ƶ�ƽ̨
            playerRoot.parent = hit.collider.transform;
            currentPlatform = hit.collider.transform;

            Debug.Log($"���ú� - PlayerRoot����: {(playerRoot.parent ? playerRoot.parent.name : "null")}");
        }
    }

    void Update()
    {
        if (currentPlatform != null)
        {
            // ��Playerλ�÷������߼��
            Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * 2.0f, Color.red, 0.1f);

            if (Physics.Raycast(ray, out RaycastHit hit, 2.0f))
            {
                // ��������Ƿ���е�ǰƽ̨����������
                Transform hitTransform = hit.collider.transform;
                bool stillOnPlatform = (hitTransform == currentPlatform) ||
                                     hitTransform.IsChildOf(currentPlatform) ||
                                     currentPlatform.IsChildOf(hitTransform);

                if (!stillOnPlatform)
                {
                    Debug.Log($"���߻�����������: {hit.collider.name}���뿪ƽ̨");
                    DetachFromPlatform();
                }
            }
            else
            {
                Debug.Log("����δ��⵽���棬�뿪ƽ̨");
                DetachFromPlatform();
            }
        }
    }

    void DetachFromPlatform()
    {
        if (playerRoot == null) return;

        Debug.Log("��ʼ����ƽ̨");

        // �ݴ浱ǰ����λ��
        Vector3 currentPosition = playerRoot.position;

        // ���븸����
        playerRoot.parent = originalParent;

        // �ָ���ˮƽ��ת��ֻ����Y����ת�����X��Z�����б��
        Vector3 eulerAngles = playerRoot.eulerAngles;
        playerRoot.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

        // ���ֵ�ǰλ��
        playerRoot.position = currentPosition;

        currentPlatform = null;
        Debug.Log($"����ƽ̨��� - PlayerRoot����: {(playerRoot.parent ? playerRoot.parent.name : "null")}");
    }
}
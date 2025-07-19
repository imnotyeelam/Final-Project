using UnityEngine;

public class FollowPlatform1 : MonoBehaviour
{
    private Transform originalParent;
    private Quaternion originalLocalRotation;
    private Transform currentPlatform;
    private CharacterController characterController;

    // ���ƽ�����ɲ���
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

        // ������ת����
        if (isTransitioning)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                rotationTransitionSpeed * Time.deltaTime);

            // ����Ƿ�ӽ�Ŀ����ת
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
                    isTransitioning = false; // ֹͣ�κν����еĹ���
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
            // ��ȡ��ǰ������ת
            Quaternion currentWorldRotation = transform.rotation;

            // �����Y����ת��ˮƽ��ת��
            float yRotation = currentWorldRotation.eulerAngles.y;

            // ���ø�����
            transform.parent = originalParent;

            // ����ֻ����Y����ת��Ŀ����ת
            // ������ǰY����ת����X��Z��ʹ��ԭʼֵ
            Vector3 originalEuler = originalLocalRotation.eulerAngles;
            targetRotation = Quaternion.Euler(originalEuler.x, yRotation, originalEuler.z);

            // ��ʼƽ�����ɵ�Ŀ����ת
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
                isTransitioning = false; // ֹͣ�κν����еĹ���
            }
        }
    }
}
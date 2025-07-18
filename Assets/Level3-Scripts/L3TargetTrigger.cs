using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    [Header("Stairs Settings")]
    public GameObject stairs; // ¥�ݶ���
    public float moveSpeed = 2f;  // ƽ�ƶ����ٶ�
    public float scaleSpeed = 2f; // ���Ŷ����ٶ�
    public Vector3 finalScale = new Vector3(1, 1, 1); // ���մ�С
    public Vector3 moveOffset = new Vector3(0, 3f, 0); // ¥��������ƫ����

    [Header("Target Settings")]
    public Animator targetAnimator; // ���� Animator
    public string hitTriggerName = "Hit"; // Animator �� Trigger ���ƣ�Ĭ�� Hit��

    private bool shouldAnimate = false;
    private Vector3 originalPos;
    private Vector3 targetPos;

    void Start()
    {
        if (stairs != null)
        {
            originalPos = stairs.transform.position;
            targetPos = originalPos + moveOffset;

            // ��ʼ���أ���С���ڵʹ���
            stairs.transform.localScale = Vector3.zero;
            stairs.transform.position = originalPos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("���ӱ����У�");

            // ���Ű��ӱ����ж���
            if (targetAnimator != null && !string.IsNullOrEmpty(hitTriggerName))
            {
                targetAnimator.SetTrigger(hitTriggerName);
            }

            // ��ʼ¥�ݶ���
            shouldAnimate = true;

            // �����ӵ�
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (shouldAnimate && stairs != null)
        {
            // ƽ������
            stairs.transform.localScale = Vector3.MoveTowards(
                stairs.transform.localScale,
                finalScale,
                scaleSpeed * Time.deltaTime
            );

            // ƽ���ƶ�
            stairs.transform.position = Vector3.MoveTowards(
                stairs.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            // �����ź�λ�ö���ɺ�ֹͣ����
            if (Vector3.Distance(stairs.transform.localScale, finalScale) < 0.01f &&
                Vector3.Distance(stairs.transform.position, targetPos) < 0.01f)
            {
                shouldAnimate = false;
                Debug.Log("¥����ȫ���֣�");
            }
        }
    }
}

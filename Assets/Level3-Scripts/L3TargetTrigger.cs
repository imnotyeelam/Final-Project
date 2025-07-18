using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    [Header("Stairs Settings")]
    public GameObject stairs; // ¥�ݶ���
    public float scaleSpeed = 2f; // ¥�������ٶ�
    public Vector3 finalScale = new Vector3(1, 1, 1); // ¥�����մ�С����ʾ��

    [Header("Target Settings")]
    public Animator targetAnimator; // ���� Animator
    public string hitTriggerName = "Hit"; // Animator �� Trigger ���ƣ�Ĭ�� Hit��

    private bool shouldAnimate = false;

    void Start()
    {
        // ��ʼ��¥�ݴ�СΪ 0�����أ�
        if (stairs != null)
        {
            stairs.transform.localScale = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("���ӱ����У�");

            // 1. ���Ű��ӱ����ж���
            if (targetAnimator != null && !string.IsNullOrEmpty(hitTriggerName))
            {
                targetAnimator.SetTrigger(hitTriggerName);
            }

            // 2. ��ʼ������¥�����ţ�
            shouldAnimate = true;

            // 3. �����ӵ�
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (shouldAnimate && stairs != null)
        {
            stairs.transform.localScale = Vector3.MoveTowards(
                stairs.transform.localScale,
                finalScale,
                scaleSpeed * Time.deltaTime
            );

            // �ж��Ƿ��������
            if (Vector3.Distance(stairs.transform.localScale, finalScale) < 0.01f)
            {
                shouldAnimate = false;
                Debug.Log("¥�ݳ�����ɣ�");
            }
        }
    }
}

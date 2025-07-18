using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    public GameObject stairs; // ¥��
    public float scaleSpeed = 2f; // �����ٶ�
    public Vector3 finalScale = new Vector3(1, 1, 1); // ���մ�С��������С��

    public Animator targetAnimator; // ���ӵ� Animator

    private bool shouldAnimate = false;

    void Start()
    {
        if (stairs != null)
        {
            stairs.transform.localScale = Vector3.zero; // ��ʼ���أ����ŵ�0��
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("���ӱ����У�");

            shouldAnimate = true;

            if (targetAnimator != null)
            {
                targetAnimator.SetTrigger("Hit"); // ���������ж���
            }

            Destroy(collision.gameObject); // �����ӵ�
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

            if (Vector3.Distance(stairs.transform.localScale, finalScale) < 0.01f)
            {
                shouldAnimate = false;
            }
        }
    }
}

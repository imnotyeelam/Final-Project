using UnityEngine;
using System.Collections;

public class L3MovingPlatformer : MonoBehaviour
{
    public Transform pointA;       // ���
    public Transform pointB;       // �յ�
    public float speed = 2f;       // �ƶ��ٶ�
    public float waitTime = 2f;    // ͣ��ʱ�䣨�룩

    private Vector3 targetPosition;

    void Start()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = pointA.position; // ��ʼλ��
            targetPosition = pointB.position;

            StartCoroutine(MovePlatform());
        }
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            // �ƶ���Ŀ���
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // �ȴ���һ֡
            }

            // ͣ�� waitTime ��
            yield return new WaitForSeconds(waitTime);

            // �л�Ŀ��λ��
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }

    // ��Ҹ���ƽ̨�ƶ�
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

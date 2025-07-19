using UnityEngine;
using System.Collections;

public class L3MovingPlane : MonoBehaviour
{
    public Transform pointA;          // ���
    public Transform pointB;          // �յ�
    public float moveSpeed = 5f;      // �ƶ��ٶ�
    public float rotateSpeed = 2f;    // ��ת�ٶ�
    public float waitTime = 2f;       // �����ͣ��
    public float extraWaitAfterRotate = 1f; // ת��180���Ķ���ͣ��

    private Transform targetPoint;
    private bool isSwitching = false; // ��ֹ�ظ��л�

    private bool canAttach = true;

    private void Start()
    {
        targetPoint = pointB; // ��ʼĿ���� pointB
    }

    private void Update()
    {
        if (isSwitching) return; // ͣ�ٻ���תʱ��ͣ�ƶ�

        // ƽ���ƶ�
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // �ж��Ƿ񵽴�Ŀ���
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f && !isSwitching)
        {
            StartCoroutine(SwitchTarget());
        }
    }

    IEnumerator SwitchTarget()
    {
        isSwitching = true;

        // �����ͣ��
        yield return new WaitForSeconds(waitTime);

        // Ŀ�곯��
        Transform nextPoint = (targetPoint == pointB) ? pointA : pointB;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);

        // ƽ����ת
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        // ת����ͣ��
        yield return new WaitForSeconds(extraWaitAfterRotate);

        // �л�Ŀ��
        targetPoint = nextPoint;
        isSwitching = false;
    }

    // ����ϵ��ɻ��ϣ����ø��ӹ�ϵ
    private void OnTriggerEnter(Collider other)
    {
        if (!canAttach) return;

        if (other.CompareTag("Player"))
        {
            PlayerController1.instance.EnterPlatform(transform);
        }
    }

    public void TemporarilyDisableAttach(float delay)
    {
        StartCoroutine(DisableAttachRoutine(delay));
    }

    IEnumerator DisableAttachRoutine(float delay)
    {
        canAttach = false;
        yield return new WaitForSeconds(delay);
        canAttach = true;
    }
}
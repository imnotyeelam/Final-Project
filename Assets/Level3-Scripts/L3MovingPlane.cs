using UnityEngine;
using System.Collections;

public class L3MovingPlane : MonoBehaviour
{
    public Transform pointA;          // 起点
    public Transform pointB;          // 终点
    public float moveSpeed = 5f;      // 移动速度
    public float rotateSpeed = 2f;    // 旋转速度
    public float waitTime = 2f;       // 到达后停顿
    public float extraWaitAfterRotate = 1f; // 转完180°后的额外停顿

    private Transform targetPoint;
    private bool isSwitching = false; // 防止重复切换

    private bool canAttach = true;

    private void Start()
    {
        targetPoint = pointB; // 初始目标是 pointB
    }

    private void Update()
    {
        if (isSwitching) return; // 停顿或旋转时暂停移动

        // 平滑移动
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // 判断是否到达目标点
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f && !isSwitching)
        {
            StartCoroutine(SwitchTarget());
        }
    }

    IEnumerator SwitchTarget()
    {
        isSwitching = true;

        // 到达后停顿
        yield return new WaitForSeconds(waitTime);

        // 目标朝向
        Transform nextPoint = (targetPoint == pointB) ? pointA : pointB;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);

        // 平滑旋转
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        // 转完再停顿
        yield return new WaitForSeconds(extraWaitAfterRotate);

        // 切换目标
        targetPoint = nextPoint;
        isSwitching = false;
    }

    // 玩家上到飞机上，设置父子关系
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
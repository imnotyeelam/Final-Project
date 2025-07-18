using UnityEngine;
using System.Collections;

public class L3MovingPlatformer : MonoBehaviour
{
    public Transform pointA;       // 起点
    public Transform pointB;       // 终点
    public float speed = 2f;       // 移动速度
    public float waitTime = 2f;    // 停顿时间（秒）

    private Vector3 targetPosition;

    void Start()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = pointA.position; // 初始位置
            targetPosition = pointB.position;

            StartCoroutine(MovePlatform());
        }
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            // 移动到目标点
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // 等待下一帧
            }

            // 停顿 waitTime 秒
            yield return new WaitForSeconds(waitTime);

            // 切换目标位置
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }

    // 玩家跟随平台移动
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

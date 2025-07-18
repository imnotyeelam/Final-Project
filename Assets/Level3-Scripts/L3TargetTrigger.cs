using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    public GameObject stairs; // 楼梯
    public float scaleSpeed = 2f; // 缩放速度
    public Vector3 finalScale = new Vector3(1, 1, 1); // 最终大小（正常大小）

    public Animator targetAnimator; // 靶子的 Animator

    private bool shouldAnimate = false;

    void Start()
    {
        if (stairs != null)
        {
            stairs.transform.localScale = Vector3.zero; // 初始隐藏（缩放到0）
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("靶子被击中！");

            shouldAnimate = true;

            if (targetAnimator != null)
            {
                targetAnimator.SetTrigger("Hit"); // 触发被击中动画
            }

            Destroy(collision.gameObject); // 销毁子弹
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

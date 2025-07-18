using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    [Header("Stairs Settings")]
    public GameObject stairs; // 楼梯对象
    public float scaleSpeed = 2f; // 楼梯缩放速度
    public Vector3 finalScale = new Vector3(1, 1, 1); // 楼梯最终大小（显示）

    [Header("Target Settings")]
    public Animator targetAnimator; // 靶子 Animator
    public string hitTriggerName = "Hit"; // Animator 的 Trigger 名称（默认 Hit）

    private bool shouldAnimate = false;

    void Start()
    {
        // 初始化楼梯大小为 0（隐藏）
        if (stairs != null)
        {
            stairs.transform.localScale = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("靶子被击中！");

            // 1. 播放靶子被击中动画
            if (targetAnimator != null && !string.IsNullOrEmpty(hitTriggerName))
            {
                targetAnimator.SetTrigger(hitTriggerName);
            }

            // 2. 开始动画（楼梯缩放）
            shouldAnimate = true;

            // 3. 销毁子弹
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

            // 判断是否完成缩放
            if (Vector3.Distance(stairs.transform.localScale, finalScale) < 0.01f)
            {
                shouldAnimate = false;
                Debug.Log("楼梯出现完成！");
            }
        }
    }
}

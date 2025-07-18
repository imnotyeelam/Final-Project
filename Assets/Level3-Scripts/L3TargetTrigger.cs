using UnityEngine;

public class L3TargetTrigger : MonoBehaviour
{
    [Header("Stairs Settings")]
    public GameObject stairs; // 楼梯对象
    public float moveSpeed = 2f;  // 平移动画速度
    public float scaleSpeed = 2f; // 缩放动画速度
    public Vector3 finalScale = new Vector3(1, 1, 1); // 最终大小
    public Vector3 moveOffset = new Vector3(0, 3f, 0); // 楼梯上升的偏移量

    [Header("Target Settings")]
    public Animator targetAnimator; // 靶子 Animator
    public string hitTriggerName = "Hit"; // Animator 的 Trigger 名称（默认 Hit）

    private bool shouldAnimate = false;
    private Vector3 originalPos;
    private Vector3 targetPos;

    void Start()
    {
        if (stairs != null)
        {
            originalPos = stairs.transform.position;
            targetPos = originalPos + moveOffset;

            // 初始隐藏（缩小并在低处）
            stairs.transform.localScale = Vector3.zero;
            stairs.transform.position = originalPos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("靶子被击中！");

            // 播放靶子被击中动画
            if (targetAnimator != null && !string.IsNullOrEmpty(hitTriggerName))
            {
                targetAnimator.SetTrigger(hitTriggerName);
            }

            // 开始楼梯动画
            shouldAnimate = true;

            // 销毁子弹
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (shouldAnimate && stairs != null)
        {
            // 平滑缩放
            stairs.transform.localScale = Vector3.MoveTowards(
                stairs.transform.localScale,
                finalScale,
                scaleSpeed * Time.deltaTime
            );

            // 平滑移动
            stairs.transform.position = Vector3.MoveTowards(
                stairs.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            // 当缩放和位置都完成后停止动画
            if (Vector3.Distance(stairs.transform.localScale, finalScale) < 0.01f &&
                Vector3.Distance(stairs.transform.position, targetPos) < 0.01f)
            {
                shouldAnimate = false;
                Debug.Log("楼梯完全出现！");
            }
        }
    }
}

using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Boss2Controller bossController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家进入 Boss 区域");

            if (PuzzleTracker.Instance.collectedPieces >= 2)
            {
                Debug.Log("条件满足：触发 Boss 起身动画");
                bossController.StartGetUpAnimation();
                gameObject.SetActive(false); // 禁用触发器，避免重复触发
            }
            else
            {
                Debug.Log("拼图不足，Boss 不起身");
            }
        }
    }
}

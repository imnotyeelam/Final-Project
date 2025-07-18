using UnityEngine;

public class Target1 : MonoBehaviour
{
    public int targetID; // 1 = 门, 2 = 平台, 3 = 门变色
    public TargetManager1 manager;
    public Animator anim2;
    private bool hasBeenHit = false; // 防止重复触发
    void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            anim2.SetBool("Hit", true); // 每次都播放受击动画
            Debug.Log("打到靶子");

            if (!hasBeenHit) // 只有第一次才通知 manager
            {
                manager.OnTargetHit(targetID);
                hasBeenHit = true; // 标记为已命中，后续不再调用 manager
            }
        }
    }
}

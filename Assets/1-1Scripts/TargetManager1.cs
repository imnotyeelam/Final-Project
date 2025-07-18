using UnityEngine;

public class TargetManager1 : MonoBehaviour
{
    [Header("机关动画控制")]
    public Animator doorAnimator;//橱柜门开关动画
    public Animator platformAnimator;//移动肥皂动画

    [Header("变色门相关")]
    public GameObject originalDoor; // 原门
    public GameObject newDoor;      // 新门（变色或其他样式）

    [Header("计数系统")]
    public int totalTargets = 3;
    private int currentHitCount = 0;
    private bool doorChanged = false;


    public void OnTargetHit(int targetID)
    {
       

        // 如果打中第一个靶子，触发门动画
        if (targetID == 1)
        {
            if (doorAnimator != null)
               
                doorAnimator.SetTrigger("Open");
        }

        // 如果打中第二个靶子，触发平台移动
        if (targetID == 2)
        {
            if (platformAnimator != null)
                platformAnimator.SetTrigger("Move");
        }

        // 第三个靶子或满足3次命中，改变门颜色
        if(targetID == 3)
        {
            currentHitCount++;
            if (currentHitCount >= totalTargets && !doorChanged)
            {
                ChangeDoor();
            }
        }
        
    }

    private void ChangeDoor()
    {
        if (originalDoor != null)
        {
            Destroy(originalDoor);
            if (newDoor != null)
            {
                newDoor.SetActive(true);
                Debug.Log("换门成功！");

            }
        }

        doorChanged = true;//先放外面防止重复
    }
}

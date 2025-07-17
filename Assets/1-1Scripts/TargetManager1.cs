using UnityEngine;

public class TargetManager1 : MonoBehaviour
{
    [Header("机关动画控制")]
    public Animator doorAnimator;//橱柜门开关动画
    public Animator platformAnimator;//移动肥皂动画

    [Header("变色门相关")]
    public Renderer colorChangingDoorRenderer;
    public Color targetColor;

    [Header("计数系统")]
    public int totalTargets = 3;
    private int currentHitCount = 0;
    private bool colorChanged = false;

    public void OnTargetHit(int targetID)
    {
        currentHitCount++;

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
        if (currentHitCount >= totalTargets && !colorChanged)
        {
            ChangeDoorColor();
        }
    }

    private void ChangeDoorColor()
    {
        if (colorChangingDoorRenderer != null)
        {
            colorChangingDoorRenderer.material.color = targetColor;
            colorChanged = true;
        }
    }
}

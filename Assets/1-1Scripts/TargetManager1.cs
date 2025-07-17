using UnityEngine;

public class TargetManager1 : MonoBehaviour
{
    [Header("���ض�������")]
    public Animator doorAnimator;//�����ſ��ض���
    public Animator platformAnimator;//�ƶ�������

    [Header("��ɫ�����")]
    public Renderer colorChangingDoorRenderer;
    public Color targetColor;

    [Header("����ϵͳ")]
    public int totalTargets = 3;
    private int currentHitCount = 0;
    private bool colorChanged = false;

    public void OnTargetHit(int targetID)
    {
        currentHitCount++;

        // ������е�һ�����ӣ������Ŷ���
        if (targetID == 1)
        {
            if (doorAnimator != null)
               
                doorAnimator.SetTrigger("Open");
        }

        // ������еڶ������ӣ�����ƽ̨�ƶ�
        if (targetID == 2)
        {
            if (platformAnimator != null)
                platformAnimator.SetTrigger("Move");
        }

        // ���������ӻ�����3�����У��ı�����ɫ
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

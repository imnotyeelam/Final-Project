using UnityEngine;

public class TargetManager1 : MonoBehaviour
{
    [Header("���ض�������")]
    public Animator doorAnimator;//�����ſ��ض���
    public Animator platformAnimator;//�ƶ�������

    [Header("��ɫ�����")]
    public GameObject originalDoor; // ԭ��
    public GameObject newDoor;      // ���ţ���ɫ��������ʽ��

    [Header("����ϵͳ")]
    public int totalTargets = 3;
    private int currentHitCount = 0;
    private bool doorChanged = false;


    public void OnTargetHit(int targetID)
    {
       

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
                Debug.Log("���ųɹ���");

            }
        }

        doorChanged = true;//�ȷ������ֹ�ظ�
    }
}

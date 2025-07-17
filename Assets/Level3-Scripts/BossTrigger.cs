using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Boss2Controller bossController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("��ҽ��� Boss ����");

            if (PuzzleTracker.Instance.collectedPieces >= 2)
            {
                Debug.Log("�������㣺���� Boss ������");
                bossController.StartGetUpAnimation();
                gameObject.SetActive(false); // ���ô������������ظ�����
            }
            else
            {
                Debug.Log("ƴͼ���㣬Boss ������");
            }
        }
    }
}

using UnityEngine;

public class L2TargetReaction : MonoBehaviour
{
    public GameObject objectToMove; // �������ϣ�����ƶ������壬����һ���š�����
    public Vector3 moveOffset = new Vector3(0, 5, 0); // �ƶ��ľ���
    public float moveSpeed = 2f;

    private bool shouldMove = false;
    private Vector3 targetPos;

    public void TriggerAction()
    {
        Debug.Log("Trigger received, object will move!");
        if (objectToMove != null)
        {
            targetPos = objectToMove.transform.position + moveOffset;
            shouldMove = true;
        }
    }

    void Update()
    {
        if (shouldMove && objectToMove != null)
        {
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(objectToMove.transform.position, targetPos) < 0.01f)
            {
                shouldMove = false;
            }
        }
    }
}

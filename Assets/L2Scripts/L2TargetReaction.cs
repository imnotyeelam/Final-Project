using UnityEngine;

public class L2TargetReaction : MonoBehaviour
{
    public GameObject objectToMove;               // Ҫ�ƶ�������
    public Vector3 moveOffset = new Vector3(15, 0, 0); // �ƶ���ƫ����
    public float moveSpeed = 4f;

    private bool shouldMove = false;
    private Vector3 originPos;    // ��ʼλ��
    private Vector3 targetPos;    // ��ǰĿ��λ��

    private bool movedOut = false; // �Ƿ��Ѿ��ƶ���Ŀ��λ��

    void Start()
    {
        if (objectToMove != null)
        {
            originPos = objectToMove.transform.position;
        }
    }

    public void TriggerAction()
    {
        if (objectToMove == null) return;

        // �л�Ŀ��λ��
        if (!movedOut)
        {
            targetPos = originPos + moveOffset;
            movedOut = true;
        }
        else
        {
            targetPos = originPos;
            movedOut = false;
        }

        shouldMove = true;
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

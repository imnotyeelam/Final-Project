using UnityEngine;

public class L2TargetReaction : MonoBehaviour
{
    public GameObject objectToMove;
    public Vector3 moveOffset = new Vector3(0, 5, 0);     // �ƶ�����
    public Vector3 scaleOffset = new Vector3(1, 1, 1);    // ���ű仯
    public float moveSpeed = 2f;
    public float scaleSpeed = 2f;

    private bool shouldAnimate = false;
    private Vector3 originPos;
    private Vector3 targetPos;

    private Vector3 originScale;
    private Vector3 targetScale;

    private bool movedOut = false; // ��ǰ״̬���Ƿ���ƫ��״̬

    void Start()
    {
        if (objectToMove != null)
        {
            originPos = objectToMove.transform.position;
            originScale = objectToMove.transform.localScale;
        }
    }

    public void TriggerAction()
    {
        if (objectToMove == null) return;

        if (!movedOut)
        {
            targetPos = originPos + moveOffset;
            targetScale = originScale + scaleOffset;
        }
        else
        {
            targetPos = originPos;
            targetScale = originScale;
        }

        movedOut = !movedOut;
        shouldAnimate = true;
    }

    void Update()
    {
        if (shouldAnimate && objectToMove != null)
        {
            // ƽ���ƶ�λ��
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            // ƽ�����Ŵ�С
            objectToMove.transform.localScale = Vector3.MoveTowards(
                objectToMove.transform.localScale,
                targetScale,
                scaleSpeed * Time.deltaTime
            );

            // �ж��Ƿ񶼵�λ
            if (
                Vector3.Distance(objectToMove.transform.position, targetPos) < 0.01f &&
                Vector3.Distance(objectToMove.transform.localScale, targetScale) < 0.01f
            )
            {
                shouldAnimate = false;
            }
        }
    }
}

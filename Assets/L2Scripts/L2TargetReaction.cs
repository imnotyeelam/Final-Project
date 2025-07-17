using UnityEngine;

public class L2TargetReaction : MonoBehaviour
{
    public GameObject objectToMove;               // 要移动的物体
    public Vector3 moveOffset = new Vector3(15, 0, 0); // 移动的偏移量
    public float moveSpeed = 4f;

    private bool shouldMove = false;
    private Vector3 originPos;    // 初始位置
    private Vector3 targetPos;    // 当前目标位置

    private bool movedOut = false; // 是否已经移动到目标位置

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

        // 切换目标位置
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

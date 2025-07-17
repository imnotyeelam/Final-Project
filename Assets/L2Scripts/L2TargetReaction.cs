using UnityEngine;

public class L2TargetReaction : MonoBehaviour
{
    public GameObject objectToMove; // 这个是你希望被移动的物体，比如一个门、箱子
    public Vector3 moveOffset = new Vector3(0, 5, 0); // 移动的距离
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

using UnityEngine;

public class MinionAI : MonoBehaviour
{
    public Transform player;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    // ʾ��׷���߼�����ѡ��
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);
        }
    }
}

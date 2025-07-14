using UnityEngine;

public class MinionAI : MonoBehaviour
{
    public Transform player;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    // Ê¾Àý×·×ÙÂß¼­£¨¿ÉÑ¡£©
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);
        }
    }
}

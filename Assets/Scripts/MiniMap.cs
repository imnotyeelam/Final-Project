using System.Numerics;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        UnityEngine.Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

    }
}

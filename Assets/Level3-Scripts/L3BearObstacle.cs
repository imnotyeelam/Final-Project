using UnityEngine;
using UnityEngine.AI;

public class BearObstacle : MonoBehaviour
{
    private NavMeshObstacle obstacle;
    private Collider bearCollider;

    void Start()
    {
        obstacle = gameObject.AddComponent<NavMeshObstacle>();
        bearCollider = GetComponent<Collider>();

        // ����NavMeshObstacle
        obstacle.shape = NavMeshObstacleShape.Capsule; // ƥ���ܵ�Collider����
        obstacle.center = bearCollider.bounds.center - transform.position;
        obstacle.radius = GetComponent<CapsuleCollider>().radius;
        obstacle.height = GetComponent<CapsuleCollider>().height;
        obstacle.carveOnlyStationary = true; // �ؼ�����NavMesh���ڳ�һ��"��������"����
    }
}
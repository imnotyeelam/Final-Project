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

        // 配置NavMeshObstacle
        obstacle.shape = NavMeshObstacleShape.Capsule; // 匹配熊的Collider类型
        obstacle.center = bearCollider.bounds.center - transform.position;
        obstacle.radius = GetComponent<CapsuleCollider>().radius;
        obstacle.height = GetComponent<CapsuleCollider>().height;
        obstacle.carveOnlyStationary = true; // 关键！在NavMesh上挖出一个"不可行走"区域
    }
}
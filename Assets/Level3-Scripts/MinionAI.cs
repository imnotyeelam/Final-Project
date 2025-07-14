using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    public float attackRange = 14.5f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = 10f; // ��ȷ���ã�
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        // �����ܲ�����
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("isRunning", true);
            agent.isStopped = false;
        }
        else
        {
            // ����ֹͣ�㣬���Ź�������
            animator.SetBool("isRunning", false);
            agent.isStopped = true;

            if (agent.remainingDistance <= attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    animator.SetTrigger("isAttacking");
                    Debug.Log("������ң�");
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}

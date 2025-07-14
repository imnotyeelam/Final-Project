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

        agent.stoppingDistance = 10f; // 正确设置！
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        // 播放跑步动画
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("isRunning", true);
            agent.isStopped = false;
        }
        else
        {
            // 到达停止点，播放攻击动画
            animator.SetBool("isRunning", false);
            agent.isStopped = true;

            if (agent.remainingDistance <= attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    animator.SetTrigger("isAttacking");
                    Debug.Log("攻击玩家！");
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}

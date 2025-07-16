using UnityEngine.AI;
using UnityEngine;

public class MinionAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Bullet Settings")]
    public GameObject bullet;
    public Transform firePoint;

    [Header("Attack Settings")]
    public float attackRange = 18f;
    public float minAttackDistance = 3f;     // 最小攻击距离，避免出生直接攻击
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    [Header("Spawn Settings")]
    public float spawnAttackDelay = 1.5f;    // 刚生成后等待多久才允许攻击
    private float spawnTime;

    private bool hasShotInThisAttack = false; // 防止一段动画射多次子弹

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = 15f;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("isRunning", true);
            agent.isStopped = false;
        }
        else
        {
            animator.SetBool("isRunning", false);
            agent.isStopped = true;

            //攻击条件：1. 冷却结束  2. 出生后延迟结束  3. 在攻击距离内  4. 大于最小攻击距离
            if (Time.time - spawnTime >= spawnAttackDelay &&
                Time.time - lastAttackTime >= attackCooldown &&
                agent.remainingDistance <= attackRange &&
                agent.remainingDistance > minAttackDistance)
            {
                animator.SetTrigger("isAttacking");
                lastAttackTime = Time.time;
                hasShotInThisAttack = false; // 重置射击标记
            }
        }

        HandleShootingByState();
    }

    void HandleShootingByState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 检查当前是不是攻击动画
        if (stateInfo.IsName("ShootSingleshot_RF01"))
        {
            //朝向玩家
            Vector3 lookPos = target.position + new Vector3(0f, 0f, -0.8f);
            lookPos.y = transform.position.y; // 保持水平旋转
            transform.LookAt(lookPos);

            float normalizedTime = stateInfo.normalizedTime % 1; //normalise the time of the animation

            if (normalizedTime > 0.3f && !hasShotInThisAttack)
            {
                Shoot();
                hasShotInThisAttack = true;
            }
        }
    }

    void Shoot()
    {
        if (firePoint == null || bullet == null) return;

        GameObject bulletObj = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * 25f;
        }
    }
}
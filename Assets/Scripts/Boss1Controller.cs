using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public enum BossState { Idle, Chasing, Attacking }
    private BossState currentState = BossState.Idle;

    private Vector3 targetPoint;
    private NavMeshAgent agent;

    public float distanceToChase = 20f;
    public float distanceToLose = 20f;
    public float distanceToStop = 6f;

    [Header("Bullet Section")]
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float waitBetweenShots = 1f;
    public float timeToShoot = 2f;
    private float fireCount, shootWaitCounter, shootTimeCounter;

    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shootTimeCounter = timeToShoot;
        shootWaitCounter = waitBetweenShots;
    }

    void Update()
    {
        if (Player.instance == null) return;

        targetPoint = Player.instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, targetPoint);

        switch (currentState)
        {
            case BossState.Idle:
                agent.isStopped = true;
                anim.SetBool("isMoving", false);

                if (distanceToPlayer <= distanceToChase)
                {
                    currentState = BossState.Chasing;
                }
                break;

            case BossState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(targetPoint);
                anim.SetBool("isMoving", true);

                if (distanceToPlayer > distanceToLose)
                {
                    currentState = BossState.Idle;
                    agent.ResetPath(); // 不回家，只停下
                    anim.SetBool("isMoving", false);
                }
                else if (distanceToPlayer <= distanceToStop)
                {
                    currentState = BossState.Attacking;
                    agent.ResetPath(); // ✅ 停止移动
                    agent.isStopped = true; // ✅ 更明确地停住
                    anim.SetBool("isMoving", false);
                    shootWaitCounter = waitBetweenShots;
                }
                
                break;

            case BossState.Attacking:
                agent.isStopped = true;
                transform.LookAt(new Vector3(Player.instance.transform.position.x, transform.position.y, Player.instance.transform.position.z));

                if (distanceToPlayer > distanceToStop && distanceToPlayer <= distanceToLose)
                {
                    currentState = BossState.Chasing;
                    break;
                }
                else if (distanceToPlayer > distanceToLose)
                {
                    currentState = BossState.Idle;
                    agent.ResetPath();
                    anim.SetBool("isMoving", false);
                    break;
                }

                if (shootWaitCounter > 0)
                {
                    shootWaitCounter -= Time.deltaTime;

                    if (shootWaitCounter <= 0)
                    {
                        shootTimeCounter = timeToShoot;
                    }
                }
                else if (Player.instance.gameObject.activeInHierarchy)
                {
                    shootTimeCounter -= Time.deltaTime;

                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(targetPoint + new Vector3(0f, 0.4f, 0f));

                            Vector3 targetDir = Player.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);
                                anim.SetTrigger("fireShot");
                            }
                            else
                            {
                                shootWaitCounter = waitBetweenShots;
                            }
                        }
                    }
                    else
                    {
                        shootWaitCounter = waitBetweenShots;
                    }
                }

                break;
        }
    }
}

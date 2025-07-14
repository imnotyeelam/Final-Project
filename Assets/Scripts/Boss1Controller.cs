using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public enum BossState { Idle, Chasing, Attacking }
    private BossState currentState = BossState.Idle;

    private Vector3 targetPoint, startPoint;
    private NavMeshAgent agent;

    public float distanceToChase = 10f;
    public float distanceToLose = 15f;
    public float distanceToStop = 2f;

    public float keepChasingTime = 5f;
    private float chaseCounter;

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
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shootWaitCounter = waitBetweenShots;
    }

    void Update()
    {
        if (Player.instance == null) return;

        targetPoint = Player.instance.transform.position;
        targetPoint.y = transform.position.y;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPoint);

        switch (currentState)
        {
            case BossState.Idle:
                anim.SetBool("isMoving", false);

                if (distanceToPlayer < distanceToChase)
                {
                    currentState = BossState.Chasing;
                }
                break;

            case BossState.Chasing:
                agent.SetDestination(targetPoint);
                anim.SetBool("isMoving", true);

                if (distanceToPlayer > distanceToLose)
                {
                    currentState = BossState.Idle;
                    agent.SetDestination(startPoint);
                }
                else if (distanceToPlayer <= distanceToStop)
                {
                    currentState = BossState.Attacking;
                    shootWaitCounter = waitBetweenShots;
                    agent.ResetPath(); // Stop moving to shoot
                    anim.SetBool("isMoving", false);
                }
                break;

            case BossState.Attacking:
                transform.LookAt(Player.instance.transform.position);

                if (distanceToPlayer > distanceToStop)
                {
                    currentState = BossState.Chasing;
                    break;
                }

                // Attack logic
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

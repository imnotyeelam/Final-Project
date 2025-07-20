using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    public enum BossState { Idle, Chasing, Attacking }
    private BossState currentState = BossState.Idle;

    private NavMeshAgent agent;
    private Vector3 targetPoint;

    public float distanceToChase;
    public float distanceToLose;
    public float agentDistanceToStop; // Renamed here


    [Header("Bullet Section")]
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float waitBetweenShots = 0.1f;
    public float timeToShoot = 0.5f;
    private float fireCount, shootWaitCounter, shootTimeCounter;

    public Animator anim;

    [Header("Audio")]
    public AudioSource footstepAudioSource;


    public void ThrowGrenade()
    {
        Vector3 targetPos = PlayerController1.instance.transform.position + new Vector3(0f, 0.4f, 0f);
        Vector3 direction = (targetPos - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        Instantiate(bullet, firePoint.position, rotation);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shootTimeCounter = timeToShoot;
        shootWaitCounter = waitBetweenShots;

        // 初始化 AudioSource 设置
        if (footstepAudioSource != null)
        {
            footstepAudioSource.loop = true;
            footstepAudioSource.playOnAwake = false;
        }
    }



    void Update()
    {
        if (PlayerController1.instance == null) return;

        targetPoint = PlayerController1.instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, targetPoint);

        switch (currentState)
        {
            case BossState.Idle:

                if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Stop();
                }
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

                // 播放跑步音效
                if (footstepAudioSource != null && !footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play();
                }

                if (distanceToPlayer <= agentDistanceToStop)
                {
                    currentState = BossState.Attacking;
                    agent.isStopped = true;
                    agent.ResetPath();
                    anim.SetBool("isMoving", false);

                    // 停止跑步音效
                    if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                    {
                        footstepAudioSource.Stop();
                    }

                    shootWaitCounter = waitBetweenShots;
                }
                else if (distanceToPlayer > distanceToLose)
                {
                    currentState = BossState.Idle;
                    agent.ResetPath();
                    anim.SetBool("isMoving", false);

                    // 停止跑步音效
                    if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                    {
                        footstepAudioSource.Stop();
                    }
                }
                break;

            case BossState.Attacking:
                transform.LookAt(new Vector3(PlayerController1.instance.transform.position.x, transform.position.y, PlayerController1.instance.transform.position.z));
                agent.isStopped = true;

                if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Stop();
                }

                // Changed to agentDistanceToStop
                if (distanceToPlayer > agentDistanceToStop && distanceToPlayer <= distanceToLose)
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
                else if (PlayerController1.instance.gameObject.activeInHierarchy)
                {
                    shootTimeCounter -= Time.deltaTime;

                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(targetPoint + new Vector3(0f, 0.4f, 0f));
                            Vector3 targetDir = PlayerController1.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
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
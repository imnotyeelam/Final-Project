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
    public float minAttackDistance = 3f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    [Header("Spawn Settings")]
    public float spawnAttackDelay = 1.5f;
    private float spawnTime;

    private bool hasShotInThisAttack = false;

    [Header("Audio Clips")]
    public AudioClip runClip;       // 跑步音效（循环）
    public AudioClip attackClip;    //攻击音效

    private AudioSource audioSource; // 通用AudioSource
    private bool isRunningSoundPlaying = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }

        agent.stoppingDistance = 15f;
        spawnTime = Time.time;

        // 初始化AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D音效
        audioSource.playOnAwake = false;
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
            PlayRunSound();
        }
        else
        {
            animator.SetBool("isRunning", false);
            agent.isStopped = true;
            StopRunSound();

            if (Time.time - spawnTime >= spawnAttackDelay &&
                Time.time - lastAttackTime >= attackCooldown &&
                agent.remainingDistance <= attackRange &&
                agent.remainingDistance > minAttackDistance)
            {
                animator.SetTrigger("isAttacking");
                lastAttackTime = Time.time;
                hasShotInThisAttack = false;

                PlayOneShot(attackClip);
            }
        }

        HandleShootingByState();
    }

    void HandleShootingByState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("ShootSingleshot_RF01"))
        {
            Vector3 lookPos = target.position + new Vector3(0f, 0.6f, -1.8f);
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);

            float normalizedTime = stateInfo.normalizedTime % 1;

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

    // 播放跑步音效（循环）
    void PlayRunSound()
    {
        if (!isRunningSoundPlaying && runClip != null)
        {
            audioSource.clip = runClip;
            audioSource.loop = true;
            audioSource.Play();
            isRunningSoundPlaying = true;
        }
    }

    void StopRunSound()
    {
        if (isRunningSoundPlaying)
        {
            audioSource.Stop();
            isRunningSoundPlaying = false;
        }
    }

    // 播放一次性音效（死亡）
    void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;

    public float rangeToTargetPlayer, timeBetweenShots = 0.5f;
    private float shotCounter;

    public Transform gun, firePoint;

    public float rotateSpeed = 45f;

    public LayerMask obstacleMask;

    [Header("Audio Clips")]
    public AudioClip attackClip; // 攻击音效

    private AudioSource audioSource;

    bool CanSeePlayer()
    {
        Vector3 direction = (Level3PlayerController1.instance.transform.position - firePoint.position).normalized;
        float distance = Vector3.Distance(firePoint.position, Level3PlayerController1.instance.transform.position);

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, distance, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        shotCounter = timeBetweenShots;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D音效
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Level3PlayerController1.instance.transform.position) < rangeToTargetPlayer && CanSeePlayer())
        {
            gun.LookAt(Level3PlayerController1.instance.transform.position + new Vector3(0f, 0.2f, 0f));

            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);

                //播放音效
                if (attackClip != null)
                {
                    audioSource.PlayOneShot(attackClip);
                }

                shotCounter = timeBetweenShots;
            }
        }
        else
        {
            shotCounter = timeBetweenShots;
            gun.rotation = Quaternion.Lerp(
                gun.rotation,
                Quaternion.Euler(0f, gun.rotation.eulerAngles.y + 10f, 0f),
                rotateSpeed * Time.deltaTime
            );
        }
    }
}

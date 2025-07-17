using UnityEngine;

public class GunShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float fireRate = 0.1f;

    [Header("Visual Effects")]
    public LightningEffect lightningPrefab;
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;

    [Header("Audio")]
    public AudioClip shootClip;
    [Range(0, 1)] public float volume = 0.7f;

    private AudioSource audioSource;
    private float nextFireTime;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    [System.Obsolete]
    void Update()
    {
        if (HandSwitcher.CurrentMode == HandSwitcher.Mode.Gun && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    [System.Obsolete]
    void Shoot()
    {
        // Calculate direction based on screen center
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 target = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(100f);
        Vector3 dir = (target - firePoint.position).normalized;

        // Instantiate and shoot bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = dir * bulletSpeed;
        }

        // Lightning effect
        if (lightningPrefab != null)
        {
            LightningEffect l = Instantiate(lightningPrefab);
            l.ShowLightning(firePoint.position, target);
        }

        // Muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
            Destroy(flash, muzzleFlashDuration);
        }

        // Audio
        if (shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}

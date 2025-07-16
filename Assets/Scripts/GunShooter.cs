using UnityEngine;
using UnityEngine.UI;

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

    [Header("Crosshair")]
    public Image crosshairImage; // Changed to direct Image reference
    public Color normalCrosshairColor = Color.white;
    public Color aimingCrosshairColor = Color.red;

    private AudioSource audioSource;
    private float nextFireTime;
    private Camera mainCamera;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
        }

        mainCamera = Camera.main;
        
        // Initialize crosshair as hidden
        if (crosshairImage != null)
        {
            crosshairImage.enabled = false;
        }
    }

    [System.Obsolete]
    void Update()
    {
        UpdateCrosshair();
        
        // Only process shooting in gun mode (mode 3)
        if (HandSwitcher.CurrentMode == 3)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    [System.Obsolete]
    void Shoot()
    {
        // Calculate shot direction from center of screen
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(100f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // Create and launch bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Visual effects
        if (lightningPrefab != null)
        {
            LightningEffect lightning = Instantiate(lightningPrefab);
            lightning.ShowLightning(firePoint.position, targetPoint);
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
            Destroy(flash, muzzleFlashDuration);
        }

        // Play sound
        if (shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }

    void UpdateCrosshair()
    {
        if (crosshairImage == null) return;
        
        // Only show in gun aim mode
        bool shouldShow = HandSwitcher.CurrentMode == 3 && HandSwitcher.IsAiming;
        crosshairImage.enabled = shouldShow;

        if (shouldShow)
        {
            // Update color based on aiming state
            crosshairImage.color = HandSwitcher.IsAiming ? aimingCrosshairColor : normalCrosshairColor;

            // Optional: Add scaling effect when aiming
            float scale = HandSwitcher.IsAiming ? 0.8f : 1f;
            crosshairImage.transform.localScale = Vector3.one * scale;
        }
    }

    void OnDisable()
    {
        // Hide crosshair when disabled
        if (crosshairImage != null)
        {
            crosshairImage.enabled = false;
        }
    }
}
using System.Collections;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;  // Max ammo capacity
    private static int currentAmmo;  // Shared ammo across all gun modes

    [Header("Visual Effects")]
    public LightningEffect lightningPrefab;
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;

    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip ammoCollectClip;  // Ammo collection sound
    [Range(0, 1)] public float volume = 0.7f;

    private AudioSource audioSource;
    private float nextFireTime;
    private Camera mainCamera;

    private Coroutine hideOutOfAmmoCoroutine;

    private static bool isAmmoInitialized = false;

    void Start()
    {
        mainCamera = Camera.main;

        if (!isAmmoInitialized)
        {
            currentAmmo = maxAmmo;
            isAmmoInitialized = true;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 0f;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
    }


    [System.Obsolete]
    public void TryShoot()
    {
        if (currentAmmo <= 0)
        {
            ShowOutOfAmmoUI(); // Only show when truly no ammo
            return;
        }

        // Only shoot if ammo > 0
        currentAmmo--;
        UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
        Shoot(); // Your shooting logic
    }

    [System.Obsolete]
    void Shoot()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 target = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(100f);
        Vector3 dir = (target - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = dir * bulletSpeed;
        }

        if (lightningPrefab != null)
        {
            LightningEffect l = Instantiate(lightningPrefab);
            l.ShowLightning(firePoint.position, target);
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
            Destroy(flash, muzzleFlashDuration);
        }

        if (shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }

        if (bulletPrefab && firePoint)
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);  // Add small increment
        UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);

        if (currentAmmo > 0)
            UIManager.Instance.ShowOutOfAmmo(false);

        // Play ammo collect sound
        if (ammoCollectClip != null)
        {
            audioSource.PlayOneShot(ammoCollectClip);
        }
    }

    public int GetCurrentAmmo() => currentAmmo;

    private void ShowOutOfAmmoUI()
    {
        // Always restart the coroutine to ensure UI shows again
        if (hideOutOfAmmoCoroutine != null)
            StopCoroutine(hideOutOfAmmoCoroutine);

        UIManager.Instance.ShowOutOfAmmo(true);
        hideOutOfAmmoCoroutine = StartCoroutine(HideOutOfAmmoAfterDelay());
    }

    private IEnumerator HideOutOfAmmoAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.ShowOutOfAmmo(false);
    }
}

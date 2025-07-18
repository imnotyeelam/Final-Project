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

    void Start()
    {
        mainCamera = Camera.main;
        if (currentAmmo == 0) currentAmmo = maxAmmo;  // Initialize ammo only once

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 0f;  // 2D sound
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
    }

    [System.Obsolete]
    void Update()
    {
        if (HandSwitcher.CurrentMode == HandSwitcher.Mode.Gun && HandSwitcher.IsAiming)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0)
                {
                    Shoot();
                    currentAmmo--;
                    UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);

                    if (currentAmmo == 0)
                        ShowOutOfAmmoUI();
                }
                else
                {
                    ShowOutOfAmmoUI();
                }

                nextFireTime = Time.time + fireRate;
            }
        }
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
        UIManager.Instance.ShowOutOfAmmo(true);

        if (hideOutOfAmmoCoroutine != null)
            StopCoroutine(hideOutOfAmmoCoroutine);

        hideOutOfAmmoCoroutine = StartCoroutine(HideOutOfAmmoAfterDelay());
    }

    private IEnumerator HideOutOfAmmoAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.ShowOutOfAmmo(false);
    }
}

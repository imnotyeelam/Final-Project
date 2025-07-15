using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public LightningEffect lightningPrefab;
    public GameObject muzzleFlashPrefab; // <- NEW

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Fire bullet
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Lightning zap effect (LineRenderer)
        if (lightningPrefab != null)
        {
            LightningEffect lightning = Instantiate(lightningPrefab);
            Vector3 endPoint = firePoint.position + firePoint.forward * 5f;
            lightning.ShowLightning(firePoint.position, endPoint);
        }

        // Particle burst (Muzzle Flash)
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 1f); // Clean up after a second
        }
    }
}

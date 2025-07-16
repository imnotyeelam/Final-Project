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
        // 1. Perform a raycast to get the correct target point
        Camera cam = Camera.main;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f); // If no hit, shoot far
        }

        // 2. Calculate the direction
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // 3. Instantiate bullet and rotate it to face the direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        // 4. Lightning Effect along the same path
        if (lightningPrefab != null)
        {
            LightningEffect lightning = Instantiate(lightningPrefab);
            lightning.ShowLightning(firePoint.position, targetPoint);
        }

        // 5. Muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 1f);
        }
    }
}

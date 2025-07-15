using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public LightningEffect lightningPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Instantiate lightning
        LightningEffect lightning = Instantiate(lightningPrefab);
        Vector3 endPoint = firePoint.position + firePoint.forward * 5f;
        lightning.ShowLightning(firePoint.position, endPoint);
    }
}

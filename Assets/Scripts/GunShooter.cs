using UnityEngine;
using DigitalRuby.LightningBolt; // Namespace from the asset

public class GunShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject lightningPrefab; // prefab that has LightningBoltScript

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Fire bullet
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Trigger lightning
        GameObject lightningGO = Instantiate(lightningPrefab);
        LightningBoltScript lightning = lightningGO.GetComponent<LightningBoltScript>();
        if (lightning != null)
        {
            lightning.StartObject = firePoint.gameObject;
            lightning.EndObject = null; // optional, use EndPosition if targeting air
            lightning.StartPosition = firePoint.position;
            lightning.EndPosition = firePoint.position + firePoint.forward * 10f;
        }
    }
}

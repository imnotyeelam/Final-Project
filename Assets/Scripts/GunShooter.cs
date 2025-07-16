using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public LightningEffect lightningPrefab;
    public GameObject muzzleFlashPrefab;
    public AudioClip shootClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
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
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        if (lightningPrefab != null)
        {
            LightningEffect lightning = Instantiate(lightningPrefab);
            lightning.ShowLightning(firePoint.position, targetPoint);
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 1f);
        }

        // 🔊 Play gun sound
        if (shootClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}

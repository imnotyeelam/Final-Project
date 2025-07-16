using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;
    public int damage = 1;
    public GameObject hitEffectPrefab;

    [Header("Crosshair Alignment")]
    public bool alignWithCrosshair = true; // Enable/disable this feature
    public float maxDistance = 100f; // Max distance if no hit is detected
    private Vector3 shootDirection;

    void Start()
    {
        Destroy(gameObject, lifetime);
        
        if (alignWithCrosshair)
        {
            // Calculate direction only once at creation
            CalculateShootDirection();
        }
    }

    void Update()
    {
        if (alignWithCrosshair)
        {
            // Move in the pre-calculated direction
            transform.position += shootDirection * speed * Time.deltaTime;
        }
        else
        {
            // Original movement behavior
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void CalculateShootDirection()
    {
        // Only do this if we're in gun mode
        if (HandSwitcher.CurrentMode == 3)
        {
            Camera playerCamera = Camera.main;
            if (playerCamera != null)
            {
                // Create ray from center of screen
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    // Calculate direction to hit point
                    shootDirection = (hit.point - transform.position).normalized;
                }
                else
                {
                    // Calculate direction to max distance point
                    Vector3 targetPoint = ray.GetPoint(maxDistance);
                    shootDirection = (targetPoint - transform.position).normalized;
                }
                
                // Align bullet rotation with direction
                transform.rotation = Quaternion.LookRotation(shootDirection);
            }
        }
        else
        {
            // Fallback to forward direction if not in gun mode
            shootDirection = transform.forward;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Spawn hit effect at point of contact
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // Damage logic
        if (other.CompareTag("Enemy") || other.CompareTag("Shootable"))
        {
            SimpleEnemy target = other.GetComponent<SimpleEnemy>();
            if (target != null)
            {
                target.TakeDamage(damage);

                if (target.health <= 0)
                {
                    Destroy(other.gameObject);
                }
            }
            else
            {
                // If no health script, destroy anyway (generic shootable object)
                Destroy(other.gameObject);
            }
        }

        // Always destroy bullet on impact
        Destroy(gameObject);
    }
}
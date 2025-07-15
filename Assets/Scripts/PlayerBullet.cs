using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;
    public int damage = 1;

    public GameObject hitEffectPrefab;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
            // Check if the target has a SimpleEnemy script or similar
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

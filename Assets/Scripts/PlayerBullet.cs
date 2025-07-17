using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;
    public int damage = 1;

    public GameObject hitEffectPrefab;
    public AudioClip hitSound; // Assign this in Inspector
    public float hitVolume = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn hit VFX
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        // Debug: Force sound to play at camera to confirm it works
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitVolume);

        // Damage logic
        if (other.CompareTag("Enemy") || other.CompareTag("Shootable"))
        {
            SimpleEnemy enemy = other.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (enemy.health <= 0)
                    Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;
    public int baseDamage = 1;

    public GameObject hitEffectPrefab;
    public AudioClip hitSound;
    public float hitVolume = 1f;

    private PlayerAttack playerAttack;

    void Start()
    {
        Destroy(gameObject, lifetime);
        playerAttack = GameObject.FindWithTag("Player")?.GetComponent<PlayerAttack>();

        if (playerAttack == null)
            Debug.LogWarning("PlayerAttack not found on tagged Player!");
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitVolume);

        if (other.CompareTag("Enemy") || other.CompareTag("Shootable"))
        {
            int finalDamage = baseDamage;

            if (playerAttack != null)
                finalDamage = Mathf.RoundToInt(baseDamage * playerAttack.attackMultiplier);

            SimpleEnemy enemy = other.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(finalDamage);
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

using UnityEngine;

public class Boss1antitank : MonoBehaviour, Boss1IDamageable
{
    public float moveSpeed, lifeTime;
    private Rigidbody rb;
    public GameObject LaserImpact;
    public int damage = 2;

    public bool attackPlayer;

    public bool damageEnemy, damagePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Boss1IDamageable damageable = other.GetComponent<Boss1IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, attackPlayer);
        }

        float offset = 0.7f;
        Vector3 newPosition = transform.position - transform.forward * offset;

        Instantiate(LaserImpact, newPosition, transform.rotation);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, bool attackPLayer)
    {
        throw new System.NotImplementedException();
    }
}

using UnityEngine;

public class Boss1antitank : MonoBehaviour, Boss1IDamageable
{
    public float moveSpeed = 10f;
    public float lifeTime = 5f;
    public float explosionRadius = 5f;
    public int damage = 2;

    public GameObject LaserImpact;

    private Rigidbody rb;
    public bool attackPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Explode();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // 只要触碰到任何物体就爆炸
        Explode();
    }

    private void Explode()
    {
        // 1. 爆炸特效
        if (LaserImpact != null)
        {
            Instantiate(LaserImpact, transform.position, Quaternion.identity);
        }

        // 2. 范围伤害
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Boss1IDamageable damageable = hit.GetComponent<Boss1IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, attackPlayer);
            }
        }

        // 3. 自毁
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, bool attackPlayer)
    {
        // 手雷不响应伤害
    }

    // 可视化爆炸半径（仅编辑器中看到）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

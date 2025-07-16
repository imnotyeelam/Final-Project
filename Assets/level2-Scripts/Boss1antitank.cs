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
        // ֻҪ�������κ�����ͱ�ը
        Explode();
    }

    private void Explode()
    {
        // 1. ��ը��Ч
        if (LaserImpact != null)
        {
            Instantiate(LaserImpact, transform.position, Quaternion.identity);
        }

        // 2. ��Χ�˺�
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Boss1IDamageable damageable = hit.GetComponent<Boss1IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, attackPlayer);
            }
        }

        // 3. �Ի�
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, bool attackPlayer)
    {
        // ���ײ���Ӧ�˺�
    }

    // ���ӻ���ը�뾶�����༭���п�����
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

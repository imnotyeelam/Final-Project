using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int health = 3;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    public float bulletSpeed = 15f;
    public int damage = 10;

    private Transform player;
    private float shootTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 dir = (player.position - firePoint.position).normalized;

        bullet.GetComponent<Rigidbody>().linearVelocity = dir * bulletSpeed;

        if (bullet.TryGetComponent<EnemyBullet>(out var enemyBullet))
        {
            enemyBullet.damage = damage;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
            Destroy(gameObject);
    }
}

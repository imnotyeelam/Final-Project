using UnityEngine;

public class L3BulletController : MonoBehaviour
{
    public float moveSpeed, LifeTime;
    private Rigidbody rb;
    public GameObject impactEffect;
    public int damage = 1;

    public bool damageEnemy, damagePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * moveSpeed;  //constant same speed

        LifeTime -= Time.deltaTime; //count the time

        if(LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 先检查碰撞对象
        if (other.CompareTag("Enemy") && damageEnemy)
        {
            other.gameObject.GetComponent<L3EnemyHealthController>()?.DamageEnemy(damage);
        }

        if (other.CompareTag("Player") && damagePlayer)
        {
            Debug.Log("Hit the player");

            // 针对玩家的特殊爆炸效果（抬高Y轴位置）
            Vector3 explosionPos = other.ClosestPoint(transform.position); // 获取玩家身上最近的碰撞点
            explosionPos.y += 0.8f; // 在玩家头顶0.8米处爆炸（可视情况调整）
            Instantiate(impactEffect, explosionPos, Quaternion.identity);
        }
        else // 其他情况（打中环境等）
        {
            // 在子弹当前位置稍前方生成爆炸（避免穿模）
            float offset = 0.5f;
            Vector3 explosionPos = transform.position + transform.forward * offset;
            Instantiate(impactEffect, explosionPos, transform.rotation);
        }

        Destroy(gameObject); // 销毁子弹
        }
    }

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
        // �ȼ����ײ����
        if (other.CompareTag("Enemy") && damageEnemy)
        {
            other.gameObject.GetComponent<L3EnemyHealthController>()?.DamageEnemy(damage);
        }

        if (other.CompareTag("Player") && damagePlayer)
        {
            Debug.Log("Hit the player");

            // �����ҵ����ⱬըЧ����̧��Y��λ�ã�
            Vector3 explosionPos = other.ClosestPoint(transform.position); // ��ȡ��������������ײ��
            explosionPos.y += 0.8f; // �����ͷ��0.8�״���ը���������������
            Instantiate(impactEffect, explosionPos, Quaternion.identity);
        }
        else // ������������л����ȣ�
        {
            // ���ӵ���ǰλ����ǰ�����ɱ�ը�����⴩ģ��
            float offset = 0.5f;
            Vector3 explosionPos = transform.position + transform.forward * offset;
            Instantiate(impactEffect, explosionPos, transform.rotation);
        }

        Destroy(gameObject); // �����ӵ�
        }
    }

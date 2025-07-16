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
        if (other.CompareTag("Enemy") && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<L3EnemyHealthController>().DamageEnemy(damage);
        }

        if (other.CompareTag("Player") && damagePlayer)
        {
            Debug.Log("Hit the player");
        }

        Destroy(gameObject);

        float offset = 0.7f;

        Vector3 newPosition = transform.position - transform.forward * offset; 

        Instantiate(impactEffect, transform.position, transform.rotation);
    }
}

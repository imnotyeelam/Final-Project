using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;
    private Rigidbody rb;
    public GameObject impactEffect;
    public int damage = 1;

    public bool damageEnemy, damagePlayer;

    public bool attackPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * moveSpeed;//constant same speed

        lifeTime -= Time.deltaTime;//count down the time

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        
        if (other.gameObject.tag == "HeadShot" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
        }
        if (other.CompareTag("Player")&&damagePlayer)
        {
            Debug.Log("Hit the player");
            //PlayerHealthController.instance.DamagePlayer(damage);
        }
        

        //IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        //try to interact with the other object that implements IDamageable
        /*
        if (damageable != null)
        {
            damageable.TakeDamage(damage, attackPlayer);
        }
        */
        Destroy(gameObject);

        float offset = 0.7f;

        Vector3 newPosition = transform.position - transform.forward * offset;

        Instantiate(impactEffect, transform.position, transform.rotation);
    }
}

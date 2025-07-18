using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;
    private Rigidbody rb;
    public GameObject impactEffect;
    public int damage = 1;

    public bool damageEnemy, damagePlayer;//can tick in the inspector

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
        
        if (other.CompareTag("Enemy") && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }

        if (other.CompareTag("Boss1") && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<Boss1HealthController>().DamageEnemy(damage);
        }


        if (other.gameObject.tag == "headShot" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
        }
        if (other.CompareTag("Player")&&damagePlayer)
        {
            //Debug.Log("Hit the player");
            PlayerHealthController1.instance.DamagePlayer(damage);
        }
        
        if (other.CompareTag("Target"))
        {
            L2TargetReaction reaction = other.GetComponent<L2TargetReaction>();
            if (reaction != null)
            {
                reaction.TriggerAction();
            }
        }
        
        Destroy(gameObject);

        float offset = 0.7f;

        Vector3 newPosition = transform.position - transform.forward * offset;

        Instantiate(impactEffect, transform.position, transform.rotation);

    }
}

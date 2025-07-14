using UnityEngine;

public class Boss1HealthController : MonoBehaviour
{
    public int currentHealth = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageEnemy(int BulletDamage)
    {
        currentHealth -= BulletDamage;

        if (currentHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void TakeDamage(int damage, bool attackPLayer)
    {
        if (!attackPLayer)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}

using UnityEngine;

public class PlayerHealthController1 : MonoBehaviour//, IDamageable
{
    public static PlayerHealthController1 instance;

    public int maxHealth, currentHealth;

    public float invLength = 1.0f;//1second
    private float inVCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;

       // UIController.instance.HealthSlider.maxValue = maxHealth;
        //UIController.instance.HealthSlider.value = currentHealth;
        //UIController.instance.HealthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (inVCounter > 0)
        {
            inVCounter -= Time.deltaTime;//will deduct based on time
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            FindFirstObjectByType<L3RespawnManager>().RespawnPlayer(); //respawn!!!
        }



    }

    public void TakeDamage(int damage, bool attackPlayer)
    {
        if (attackPlayer)
        {
            if (inVCounter <= 0)
            {
                currentHealth -= damage;
                if (currentHealth <= 0)
                {
                    transform.parent.gameObject.SetActive(false);//hide player

                    currentHealth = 0;

                    //GameManager.instance.PlayerDied();//reset the game after 2 seconds
                }

                inVCounter = invLength;//击中后会有1s的冷却时间,give flexibility to players

                //UIController.instance.HealthSlider.value = currentHealth;
                //UIController.instance.HealthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        //UIController.instance.HealthSlider.value = currentHealth;
        //UIController.instance.HealthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }
}

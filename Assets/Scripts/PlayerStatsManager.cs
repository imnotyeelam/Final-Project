using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxEnergy = 100;
    public int maxAmmo = 30;
    
    [Header("Current Stats")]
    public int currentHealth;
    public int currentEnergy;
    public int currentAmmo;

    public UIManager uiManager;

    public static PlayerStatsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        currentAmmo = maxAmmo;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateUI();
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
            uiManager.UpdateEnergy(currentEnergy, maxEnergy);
            uiManager.UpdateAmmo(currentAmmo, maxAmmo);
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add death handling here
    }
}
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UIManager uiManager;
    public WeaponManager weaponManager;

    private void Start()
    {
        currentHealth = maxHealth;

        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }

        if (weaponManager != null && uiManager != null)
        {
            uiManager.UpdateWeapon(weaponManager.GetCurrentWeaponName());
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // TODO: Add respawn, game over screen, etc.
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public Slider energyBar;
    public Text ammoText;

    public PlayerStatus playerStatus;
    public WeaponManager weaponManager;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerStatus != null)
        {
            healthBar.value = playerStatus.health;
            energyBar.value = playerStatus.energy;
        }

        if (weaponManager != null)
        {
            ammoText.text = "Ammo: " + weaponManager.currentAmmo + "/" + weaponManager.maxAmmo;
        }
    }
}

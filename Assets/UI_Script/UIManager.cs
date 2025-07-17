using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health UI")]
    public Text healthText;
    public Slider healthSlider;

    [Header("Energy UI")]
    public Text energyText;
    public Slider energySlider;

    [Header("Ammo UI")]
    public Text ammoText;
    public Slider ammoSlider;

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

    public void UpdateHealth(int current, int max)
    {
        if (healthText != null)
            healthText.text = $"HP: {current}/{max}";
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }

    public void UpdateEnergy(int current, int max)
    {
        if (energyText != null)
            energyText.text = $"Energy: {current}/{max}";
        if (energySlider != null)
        {
            energySlider.maxValue = max;
            energySlider.value = current;
        }
    }

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {current}/{max}";
        if (ammoSlider != null)
        {
            ammoSlider.maxValue = max;
            ammoSlider.value = current;
        }
    }
}
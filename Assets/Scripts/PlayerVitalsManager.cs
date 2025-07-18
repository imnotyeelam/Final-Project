using UnityEngine;

public class PlayerVitalsManager : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxEnergy = 100f;
    public float currentHP;
    public float currentEnergy;

    public int totalPieces = 3;
    public int collectedPieces = 0;

    [Header("Use Prop Audio Clips")]
    public AudioClip useAmmoClip;
    public AudioClip useHPClip;
    public AudioClip useEnergyClip;

    private AudioSource audioSource;
    private bool isInvincible = false; // For mom's buff

    void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
    }

    public void AddHP(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return; // Prevent damage if invincible (mom buff)
        currentHP = Mathf.Max(0, currentHP - amount);
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
    }

    public void CollectPiece()
    {
        if (collectedPieces < totalPieces)
        {
            collectedPieces++;
            UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && UIManager.Instance.UseProp("Ammo"))
        {
            FindObjectOfType<GunShooter>()?.AddAmmo(10);
            PlayClip(useAmmoClip);
        }
        if (Input.GetKeyDown(KeyCode.O) && UIManager.Instance.UseProp("HP"))
        {
            AddHP(10);
            PlayClip(useHPClip);
        }
        if (Input.GetKeyDown(KeyCode.P) && UIManager.Instance.UseProp("Energy"))
        {
            AddEnergy(10);
            PlayClip(useEnergyClip);
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource)
            audioSource.PlayOneShot(clip);
    }
}

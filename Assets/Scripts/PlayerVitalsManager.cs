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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Init UI
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

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (UIManager.Instance.UseProp("Ammo"))
            {
                Debug.Log("Used Ammo Prop");
                FindObjectOfType<GunShooter>()?.AddAmmo(10);  
                PlayClip(useAmmoClip);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (UIManager.Instance.UseProp("HP"))
            {
                Debug.Log("Used HP Prop");
                AddHP(10);
                PlayClip(useHPClip);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (UIManager.Instance.UseProp("Energy"))
            {
                Debug.Log("Used Energy Prop");
                AddEnergy(10);
                PlayClip(useEnergyClip);
            }
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private bool isInvincible = false;

    // For energy deduction timer
    private float energyTimer = 0f;
    private float energyInterval = 120f; // 2 minutes
    private float energyLoss = 5f;

    // For fall damage
    private float lastY;
    private float fallThreshold = 2.0f; // to detect actual fall
    private CharacterController controller;

    public Image hurtFlash;
    public float flashDuration = 0.5f;
    public AudioClip fallSound;


    IEnumerator FlashRed()
    {
        if (hurtFlash != null)
        {
            hurtFlash.enabled = true; // show the image
            hurtFlash.color = new Color(1, 0, 0, 0.5f); // semi-transparent red

            yield return new WaitForSeconds(flashDuration);

            hurtFlash.color = new Color(1, 0, 0, 0); // clear color
            hurtFlash.enabled = false; // hide again
        }
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

        lastY = transform.position.y;
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);

        if (hurtFlash != null)
        {
            hurtFlash.enabled = false; // keep it hidden but enabled in hierarchy
            hurtFlash.color = new Color(1, 0, 0, 0);
        }
    }


    [System.Obsolete]
    void Update()
    {
        // ENERGY DEDUCTION
        energyTimer += Time.deltaTime;
        if (energyTimer >= energyInterval)
        {
            energyTimer = 0f;
            ConsumeEnergy(energyLoss);
            Debug.Log($"[Energy Timer] -{energyLoss} energy. Current energy: {currentEnergy}");
        }

        // FALL DAMAGE DETECTION
        if (controller.isGrounded)
        {
            float fallDistance = lastY - transform.position.y;

            if (fallDistance > fallThreshold)
            {
                float damage = Mathf.Floor(fallDistance / 25f) * 10f;
                if (damage > 0)
                {
                    TakeDamage(damage);
                    Debug.Log($"[Fall Damage] Fall from {fallDistance:F1} units. Took {damage} damage. Current HP: {currentHP}");

                    if (fallSound != null && audioSource != null)
                        audioSource.PlayOneShot(fallSound); // immediate sound

                    StartCoroutine(FlashRed()); // red flash UI
                }
            }

            lastY = transform.position.y;
        }
        else
        {
            if (transform.position.y > lastY)
                lastY = transform.position.y;
        }

        // Prop debug keys (optional)
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
        if (isInvincible) return;
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

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource)
            audioSource.PlayOneShot(clip);
    }
}

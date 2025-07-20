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

    private float energyTimer = 0f;
    private float energyInterval = 120f;
    private float energyLoss = 5f;

    private float lastY;
    private float fallThreshold = 2.0f;
    private CharacterController controller;

    public Image hurtFlash;
    public float flashDuration = 0.5f;
    public AudioClip fallSound;

    [Header("Respawn Settings")]
    public Transform respawnPoint;
    private float lastEnergy;

    private void Start()
    {
        currentHP = maxHP;
        currentEnergy = maxEnergy;

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        lastY = transform.position.y;

        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);

        if (hurtFlash != null)
        {
            hurtFlash.enabled = false;
            hurtFlash.color = new Color(1, 0, 0, 0);
        }
    }

    [System.Obsolete]
    void Update()
    {
        if (Mathf.Abs(lastEnergy - currentEnergy) > 0.1f) // small threshold to avoid noise
        {
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
            lastEnergy = currentEnergy;
        }
            // Energy drain over time
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyInterval)
            {
                energyTimer = 0f;
                ConsumeEnergy(energyLoss);
            }

        // Fall damage
        if (controller.isGrounded)
        {
            float fallDistance = lastY - transform.position.y;
            if (fallDistance > fallThreshold)
            {
                float damage = Mathf.Floor(fallDistance / 25f) * 10f;
                if (damage > 0)
                {
                    TakeDamage(damage);
                    if (fallSound) audioSource.PlayOneShot(fallSound);
                    StartCoroutine(FlashRed());
                }
            }
            lastY = transform.position.y;
        }
        else if (transform.position.y > lastY)
        {
            lastY = transform.position.y;
        }

        // Debug keys for props
        if (Input.GetKeyDown(KeyCode.I)) TryUseAmmoProp();
        if (Input.GetKeyDown(KeyCode.O)) TryUseHPProp();
        if (Input.GetKeyDown(KeyCode.P)) TryUseEnergyProp();

        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);

    }

    IEnumerator FlashRed()
    {
        if (hurtFlash)
        {
            hurtFlash.enabled = true;
            hurtFlash.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(flashDuration);
            hurtFlash.color = new Color(1, 0, 0, 0);
            hurtFlash.enabled = false;
        }
    }

    void TryUseHPProp()
    {
        if (!UIManager.Instance.HasProp("HP"))
        {
            UIManager.Instance.ShowPrompt("No HP props available!");
            return;
        }

        if (currentHP >= maxHP)
        {
            UIManager.Instance.ShowPrompt("HP is already full!");
            return;
        }

        if (UIManager.Instance.UseProp("HP"))
        {
            AddHP(10);
            PlayClip(useHPClip);
        }
    }

    void TryUseEnergyProp()
    {
        if (!UIManager.Instance.HasProp("Energy"))
        {
            UIManager.Instance.ShowPrompt("No Energy props available!");
            return;
        }

        if (currentEnergy >= maxEnergy)
        {
            UIManager.Instance.ShowPrompt("Energy is already full!");
            return;
        }

        if (UIManager.Instance.UseProp("Energy"))
        {
            AddEnergy(10);
            PlayClip(useEnergyClip);
        }
    }

    [System.Obsolete]
    void TryUseAmmoProp()
    {
        if (!UIManager.Instance.HasProp("Ammo"))
        {
            UIManager.Instance.ShowPrompt("No Ammo props available!");
            return;
        }

        WeaponManager weapon = WeaponManager.Instance;
        if (weapon != null && weapon.currentAmmo >= weapon.maxAmmo)
        {
            UIManager.Instance.ShowPrompt("Ammo is already full!");
            return;
        }

        if (UIManager.Instance.UseProp("Ammo"))
        {
            WeaponManager.Instance.AddAmmo(10);
            PlayClip(useAmmoClip);
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

    [System.Obsolete]
    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHP = Mathf.Max(0, currentHP - amount);
        UIManager.Instance.UpdateHealth(currentHP, maxHP);

        if (currentHP <= 0) TriggerDeath();
    }

    [System.Obsolete]
    void TriggerDeath()
    {
        HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
        float delay = 3f;

        if (handSwitcher != null)
        {
            handSwitcher.SwitchToDeadState();
            delay = handSwitcher.fadeDuration + 1f;
        }
        StartCoroutine(RespawnAfterDelay(delay));
    }

    [System.Obsolete]
    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnPlayer();
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

    public void SetInvincible(bool value) => isInvincible = value;

    [System.Obsolete]
    public void RespawnPlayer()
    {
        if (respawnPoint != null) StartCoroutine(RespawnMoveCoroutine());

        currentHP = 50f;
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);

        GunShooter gunShooter = FindObjectOfType<GunShooter>();
        if (gunShooter) GunShooter.ResetAmmo();

        ResetProps();

        HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
        if (handSwitcher != null)
        {
            handSwitcher.SetHandMode(HandSwitcher.Mode.Idle);
            handSwitcher.ResetDeathState();
        }
    }

    IEnumerator RespawnMoveCoroutine()
    {
        SimpleFPSMovement movement = GetComponent<SimpleFPSMovement>();
        if (movement) movement.enabled = false;

        CharacterController cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        transform.position = respawnPoint.position + Vector3.up * 0.3f;
        transform.rotation = respawnPoint.rotation;
        Physics.SyncTransforms();

        yield return null;
        if (cc) cc.enabled = true;
        yield return null;
        if (movement) movement.enabled = true;
    }

    void ResetProps() => UIManager.Instance.ClearAllProps();

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource) audioSource.PlayOneShot(clip);
    }
}

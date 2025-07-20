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

    [Header("Respawn Settings")]
    public Transform respawnPoint;  // 指定复活位置

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


        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
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

        if (currentHP <= 0)
        {
            Debug.Log("[Player] HP reached 0 → trigger death sequence");
            TriggerDeath();
        }
    }

    void TriggerDeath()
    {
        HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
        float delay = 3f; // respawn after 3 seconds

        if (handSwitcher != null)
        {
            handSwitcher.SwitchToDeadState();
            delay = handSwitcher.fadeDuration + 1f; // black screen + 1 second
        }

        StartCoroutine(RespawnAfterDelay(delay));
    }

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

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    [System.Obsolete]
    public void RespawnPlayer()
    {
        Debug.Log("[Player] Respawn started...");

        // ✅ 1. Move the player safely to the respawn location
        if (respawnPoint != null)
        {
            // Use a coroutine so CharacterController can be disabled one frame
            StartCoroutine(RespawnMoveCoroutine());
        }

        // ✅ 2. Restore HP & Energy after respawn
        currentHP = 50f;  // Respawn with half HP
        UIManager.Instance.UpdateHealth(currentHP, maxHP);

        // Energy stays the same (if you want to reset too, set it manually)
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);

        // ✅ 3. Reset gun ammo and props
        GunShooter gunShooter = FindObjectOfType<GunShooter>();
        if (gunShooter != null)
        {
            GunShooter.ResetAmmo();  // Reset ammo count
        }
        ResetProps();  // Clear all temporary props (HP/Ammo/Energy items)

        // ✅ 4. Reset HandSwitcher (exit death mode, remove black screen)
        HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
        if (handSwitcher != null)
        {
            handSwitcher.SetHandMode(HandSwitcher.Mode.Idle);

            // ✅ IMPORTANT: reset death fade & flag
            handSwitcher.ResetDeathState();
        }

        Debug.Log("[Player] Respawn finished!");
    }

    // ✅ This coroutine safely moves the player without falling through the floor
    IEnumerator RespawnMoveCoroutine()
    {
        // 1. Temporarily disable movement
        SimpleFPSMovement movement = GetComponent<SimpleFPSMovement>();
        if (movement) movement.enabled = false;

        // 2. Temporarily disable CharacterController for teleport
        CharacterController cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        // 3. Teleport player slightly above respawnPoint
        transform.position = respawnPoint.position + Vector3.up * 0.3f;
        transform.rotation = respawnPoint.rotation;

        // Sync transforms so physics updates correctly
        Physics.SyncTransforms();

        // Wait 1 frame so Unity can refresh position
        yield return null;

        // 4. Re-enable CharacterController
        if (cc) cc.enabled = true;

        // 5. Re-enable movement after another frame (extra safety)
        yield return null;
        if (movement) movement.enabled = true;

        Debug.Log("[Player] Teleport complete, controller & movement restored");
    }
    void ResetProps()
    {
        // Internally zeroing them out via UIManager
        UIManager.Instance.ClearAllProps();
    }

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource)
            audioSource.PlayOneShot(clip);
    }
}

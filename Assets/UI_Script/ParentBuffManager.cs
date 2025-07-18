using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ParentBuffManager : MonoBehaviour
{
    [Header("Player Scripts")]
    public PlayerAttack playerAttack;
    public PlayerVitalsManager playerVitalsManager;

    [Header("Cooldown Texts")]
    public Text dadCooldownText;
    public Text momCooldownText;

    [Header("Buff Settings")]
    public float dadBuffDuration = 30f;
    public float dadCooldown = 120f;
    public float momBuffDuration = 5f;
    public float momCooldown = 180f;

    private bool dadBuffAvailable = true;
    private bool momBuffAvailable = true;
    public Transform someVariable;

    void Start()
    {
        if(someVariable == null)
            Debug.LogError("SomeVariable has not been assigned.", this)	;
        // Notice, that we pass 'this' as a context object so that Unity will highlight this object when clicked.
    }

    void Awake()
    {
        if (playerAttack == null)
        {
            Debug.LogError("PlayerAttack is not assigned in the Inspector.");
        }

        if (playerVitalsManager == null)
        {
            Debug.LogError("PlayerVitalsManager is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        // Press Z to activate Dad Buff (double attack)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ActivateDadBuff();
        }

        // Press X to activate Mom Buff (invincibility)
        if (Input.GetKeyDown(KeyCode.X))
        {
            ActivateMomBuff();
        }
    }

    public void ActivateDadBuff()
    {
        if (!dadBuffAvailable) return;
        StartCoroutine(DadBuffRoutine());
    }

    private IEnumerator DadBuffRoutine()
    {
        dadBuffAvailable = false;

        if (playerAttack != null)
        {
            playerAttack.attackMultiplier = 2f;
            Debug.Log("Dad Buff Activated: Attack multiplier set to 2x.");
        }

        yield return new WaitForSeconds(dadBuffDuration);

        if (playerAttack != null)
        {
            playerAttack.attackMultiplier = 1f;
            Debug.Log("Dad Buff Ended: Attack multiplier reset to 1x.");
        }

        StartCoroutine(DadCooldownRoutine());
    }

    private IEnumerator DadCooldownRoutine()
    {
        float remaining = dadCooldown;
        while (remaining > 0)
        {
            if (dadCooldownText != null)
                dadCooldownText.text = $"{Mathf.CeilToInt(remaining)}s";

            remaining -= Time.deltaTime;
            yield return null;
        }

        if (dadCooldownText != null)
            dadCooldownText.text = "";

        dadBuffAvailable = true;
    }

    public void ActivateMomBuff()
    {
        if (!momBuffAvailable) return;
        StartCoroutine(MomBuffRoutine());
    }

    private IEnumerator MomBuffRoutine()
    {
        momBuffAvailable = false;

        if (playerVitalsManager != null)
        {
            playerVitalsManager.SetInvincible(true);
            Debug.Log("Mom Buff Activated: Player is now invincible.");
        }

        yield return new WaitForSeconds(momBuffDuration);

        if (playerVitalsManager != null)
        {
            playerVitalsManager.SetInvincible(false);
            Debug.Log("Mom Buff Ended: Player is no longer invincible.");
        }

        StartCoroutine(MomCooldownRoutine());
    }

    private IEnumerator MomCooldownRoutine()
    {
        float remaining = momCooldown;
        while (remaining > 0)
        {
            if (momCooldownText != null)
                momCooldownText.text = $"{Mathf.CeilToInt(remaining)}s";

            remaining -= Time.deltaTime;
            yield return null;
        }

        if (momCooldownText != null)
            momCooldownText.text = "";

        momBuffAvailable = true;
    }
}

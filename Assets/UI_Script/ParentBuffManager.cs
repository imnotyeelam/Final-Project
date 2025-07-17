using UnityEngine;
using UnityEngine.UI;

public class ParentBuffManager : MonoBehaviour
{
    [Header("Dad Buff Settings")]
    public KeyCode dadKey = KeyCode.Alpha1;
    public float dadDuration = 30f;
    public float dadCooldown = 120f;

    [Header("Mom Buff Settings")]
    public KeyCode momKey = KeyCode.Alpha2;
    public float momDuration = 5f;
    public float momCooldown = 180f;

    [Header("Dad Buff UI")]
    public Image dadCooldownOverlay;
    public Text dadCooldownText;
    public Image dadActiveOverlay;
    public Text dadActiveText;

    [Header("Mom Buff UI")]
    public Image momCooldownOverlay;
    public Text momCooldownText;
    public Image momActiveOverlay;
    public Text momActiveText;

    private bool isDadActive = false;
    private bool isDadCooldown = false;
    private float dadTimer = 0f;
    private float dadCooldownTimer = 0f;

    private bool isMomActive = false;
    private bool isMomCooldown = false;
    private float momTimer = 0f;
    private float momCooldownTimer = 0f;

    void Start()
    {
        ResetUI();
    }

    void Update()
    {
        // Dad Buff activation
        if (Input.GetKeyDown(dadKey) && !isDadActive && !isDadCooldown)
        {
            ActivateDadBuff();
        }

        // Dad Buff active countdown
        if (isDadActive)
        {
            dadTimer -= Time.deltaTime;
            UpdateActiveUI(dadActiveOverlay, dadActiveText, dadTimer, dadDuration);

            if (dadTimer <= 0f)
                EndDadBuff();
        }

        // Dad Cooldown countdown
        if (isDadCooldown)
        {
            dadCooldownTimer -= Time.deltaTime;
            UpdateCooldownUI(dadCooldownOverlay, dadCooldownText, dadCooldownTimer, dadCooldown);

            if (dadCooldownTimer <= 0f)
            {
                isDadCooldown = false;
                HideCooldownUI(dadCooldownOverlay, dadCooldownText);
                Debug.Log("Dad Buff ready again");
            }
        }

        // Mom Buff activation
        if (Input.GetKeyDown(momKey) && !isMomActive && !isMomCooldown)
        {
            ActivateMomBuff();
        }

        // Mom Buff active countdown
        if (isMomActive)
        {
            momTimer -= Time.deltaTime;
            UpdateActiveUI(momActiveOverlay, momActiveText, momTimer, momDuration);

            if (momTimer <= 0f)
                EndMomBuff();
        }

        // Mom Cooldown countdown
        if (isMomCooldown)
        {
            momCooldownTimer -= Time.deltaTime;
            UpdateCooldownUI(momCooldownOverlay, momCooldownText, momCooldownTimer, momCooldown);

            if (momCooldownTimer <= 0f)
            {
                isMomCooldown = false;
                HideCooldownUI(momCooldownOverlay, momCooldownText);
                Debug.Log("Mom Buff ready again");
            }
        }
    }

    void ActivateDadBuff()
    {
        isDadActive = true;
        dadTimer = dadDuration;

        ShowActiveUI(dadActiveOverlay, dadActiveText);
        UpdateActiveUI(dadActiveOverlay, dadActiveText, dadTimer, dadDuration);

        Debug.Log($"DAD BUFF ACTIVATED for {dadDuration} seconds");

        // Example: double damage
        // playerWeapon.damageMultiplier = 2f;
    }

    void EndDadBuff()
    {
        isDadActive = false;
        HideActiveUI(dadActiveOverlay, dadActiveText);

        isDadCooldown = true;
        dadCooldownTimer = dadCooldown;

        ShowCooldownUI(dadCooldownOverlay, dadCooldownText);
        Debug.Log("Dad Buff ended, cooldown started");

        // playerWeapon.damageMultiplier = 1f;
    }

    void ActivateMomBuff()
    {
        isMomActive = true;
        momTimer = momDuration;

        ShowActiveUI(momActiveOverlay, momActiveText);
        UpdateActiveUI(momActiveOverlay, momActiveText, momTimer, momDuration);

        Debug.Log($"MOM BUFF ACTIVATED for {momDuration} seconds");

        // Example: invincible
        // playerHealth.isInvincible = true;
    }

    void EndMomBuff()
    {
        isMomActive = false;
        HideActiveUI(momActiveOverlay, momActiveText);

        isMomCooldown = true;
        momCooldownTimer = momCooldown;

        ShowCooldownUI(momCooldownOverlay, momCooldownText);
        Debug.Log("Mom Buff ended, cooldown started");

        // playerHealth.isInvincible = false;
    }

    // ===== UI Helpers =====

    void ShowActiveUI(Image overlay, Text text)
    {
        if (overlay) overlay.gameObject.SetActive(true);
        if (text) text.gameObject.SetActive(true);
    }

    void HideActiveUI(Image overlay, Text text)
    {
        if (overlay) overlay.gameObject.SetActive(false);
        if (text) text.gameObject.SetActive(false);
    }

    void ShowCooldownUI(Image overlay, Text text)
    {
        if (overlay) overlay.gameObject.SetActive(true);
        if (text) text.gameObject.SetActive(true);
    }

    void HideCooldownUI(Image overlay, Text text)
    {
        if (overlay) overlay.gameObject.SetActive(false);
        if (text) text.gameObject.SetActive(false);
    }

    void UpdateActiveUI(Image overlay, Text text, float remain, float maxDuration)
    {
        if (overlay)
        {
            float fillAmount = Mathf.Clamp01(remain / maxDuration);
            overlay.fillAmount = fillAmount;
        }
        if (text)
        {
            text.text = Mathf.Ceil(remain).ToString();
        }
    }

    void UpdateCooldownUI(Image overlay, Text text, float remain, float maxDuration)
    {
        if (overlay)
        {
            float fillAmount = Mathf.Clamp01(remain / maxDuration);
            overlay.fillAmount = fillAmount;
        }
        if (text)
        {
            text.text = Mathf.Ceil(remain).ToString();
        }
    }

    void ResetUI()
    {
        HideCooldownUI(dadCooldownOverlay, dadCooldownText);
        HideCooldownUI(momCooldownOverlay, momCooldownText);

        HideActiveUI(dadActiveOverlay, dadActiveText);
        HideActiveUI(momActiveOverlay, momActiveText);
    }
}

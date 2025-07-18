using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ParentBuffManager : MonoBehaviour
{
    public PlayerAttack playerAttack;
    public PlayerVitalsManager playerVitalsManager;

    [Header("Dad Buff Settings")]
    public float dadBuffDuration = 30f;
    public float dadCooldown = 120f;
    public Image dadBuffIcon;
    public Text dadCooldownText;
    public Text dadDurationText;

    [Header("Mom Buff Settings")]
    public float momBuffDuration = 5f;
    public float momCooldown = 180f;
    public Image momBuffIcon;
    public Text momCooldownText;
    public Text momDurationText;

    private bool isDadBuffActive = false;
    private bool isMomBuffActive = false;

    private Outline dadOutline;
    private Outline momOutline;

    void Awake()
    {
        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();
        if (playerVitalsManager == null)
            playerVitalsManager = GetComponent<PlayerVitalsManager>();

        dadOutline = dadBuffIcon.GetComponent<Outline>();
        momOutline = momBuffIcon.GetComponent<Outline>();

        if (dadCooldownText) dadCooldownText.text = "";
        if (momCooldownText) momCooldownText.text = "";
        if (dadDurationText) dadDurationText.text = "";
        if (momDurationText) momDurationText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isDadBuffActive)
            StartCoroutine(DadBuffRoutine());

        if (Input.GetKeyDown(KeyCode.Z) && !isMomBuffActive)
            StartCoroutine(MomBuffRoutine());
    }

    IEnumerator DadBuffRoutine()
    {
        isDadBuffActive = true;

        // Activate buff
        playerAttack.attackMultiplier = 2f;
        dadBuffIcon.color = Color.white;
        dadOutline.enabled = true;

        // Buff duration countdown
        float durationRemaining = dadBuffDuration;
        while (durationRemaining > 0)
        {
            if (dadDurationText)
                dadDurationText.text = Mathf.Ceil(durationRemaining).ToString();
            durationRemaining -= Time.deltaTime;
            yield return null;
        }

        // End buff
        playerAttack.attackMultiplier = 1f;
        dadOutline.enabled = false;
        dadBuffIcon.color = Color.grey;
        if (dadDurationText) dadDurationText.text = "";

        // Cooldown countdown
        float cooldownRemaining = dadCooldown;
        while (cooldownRemaining > 0)
        {
            if (dadCooldownText)
                dadCooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }

        dadCooldownText.text = "";
        dadBuffIcon.color = Color.white;
        isDadBuffActive = false;
    }

    IEnumerator MomBuffRoutine()
    {
        isMomBuffActive = true;

        // Activate buff
        playerVitalsManager.SetInvincible(true);
        momBuffIcon.color = Color.white;
        momOutline.enabled = true;

        // Buff duration countdown
        float durationRemaining = momBuffDuration;
        while (durationRemaining > 0)
        {
            if (momDurationText)
                momDurationText.text = Mathf.Ceil(durationRemaining).ToString();
            durationRemaining -= Time.deltaTime;
            yield return null;
        }

        // End buff
        playerVitalsManager.SetInvincible(false);
        momOutline.enabled = false;
        momBuffIcon.color = Color.grey;
        if (momDurationText) momDurationText.text = "";

        // Cooldown countdown
        float cooldownRemaining = momCooldown;
        while (cooldownRemaining > 0)
        {
            if (momCooldownText)
            {
                Debug.Log("momCooldownText is " + momCooldownText);
                momCooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
            }
            else
            {
                Debug.LogWarning("momCooldownText is NULL!");
            }

            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }


        momCooldownText.text = "";
        momBuffIcon.color = Color.white;
        isMomBuffActive = false;
    }
}

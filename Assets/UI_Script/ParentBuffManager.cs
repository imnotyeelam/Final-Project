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

    [Header("Dad UI Overlays")]
    public GameObject dadActiveOverlay;
    public GameObject dadCooldownOverlay;

    [Header("Mom UI Overlays")]
    public GameObject momActiveOverlay;
    public GameObject momCooldownOverlay;

    private bool isDadBuffActive = false;
    private bool isMomBuffActive = false;

    private Outline dadOutline;
    private Outline momOutline;

    [Header("Audio")]
    public AudioClip dadBuffSound;
    public AudioClip momBuffSound;
    public AudioSource audioSource;


    void Awake()
    {
        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();
        if (playerVitalsManager == null)
            playerVitalsManager = GetComponent<PlayerVitalsManager>();

        if (dadBuffIcon != null)
            dadOutline = dadBuffIcon.GetComponent<Outline>();
        else
            Debug.LogWarning("DadBuffIcon not assigned!");

        if (momBuffIcon != null)
            momOutline = momBuffIcon.GetComponent<Outline>();
        else
            Debug.LogWarning("MomBuffIcon not assigned!");

        if (dadOutline != null)
        {
            dadOutline.enabled = false;
            dadOutline.effectColor = Color.green;
        }

        if (momOutline != null)
        {
            momOutline.enabled = false;
            momOutline.effectColor = Color.green;
        }

        if (dadCooldownText) dadCooldownText.text = "";
        if (momCooldownText) momCooldownText.text = "";
        if (dadDurationText) dadDurationText.text = "";
        if (momDurationText) momDurationText.text = "";

        if (dadActiveOverlay) dadActiveOverlay.SetActive(false);
        if (dadCooldownOverlay) dadCooldownOverlay.SetActive(false);
        if (momActiveOverlay) momActiveOverlay.SetActive(false);
        if (momCooldownOverlay) momCooldownOverlay.SetActive(false);
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
        if (playerAttack != null)
            playerAttack.attackMultiplier = 2f;
        if (audioSource && dadBuffSound)
            audioSource.PlayOneShot(dadBuffSound);
        if (dadBuffIcon != null)
            dadBuffIcon.color = Color.white;
        if (dadOutline != null)
        {
            dadOutline.effectColor = Color.green;
            dadOutline.enabled = true;
        }
        if (dadActiveOverlay) dadActiveOverlay.SetActive(true);

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
        if (playerAttack != null)
            playerAttack.attackMultiplier = 1f;
        if (dadOutline != null)
            dadOutline.enabled = false;
        if (dadBuffIcon != null)
            dadBuffIcon.color = Color.grey;
        if (dadDurationText) dadDurationText.text = "";
        if (dadActiveOverlay) dadActiveOverlay.SetActive(false);
        if (dadCooldownOverlay) dadCooldownOverlay.SetActive(true);

        // Cooldown countdown
        float cooldownRemaining = dadCooldown;
        while (cooldownRemaining > 0)
        {
            if (dadCooldownText)
                dadCooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }

        if (dadCooldownText) dadCooldownText.text = "";
        if (dadCooldownOverlay) dadCooldownOverlay.SetActive(false);
        if (dadBuffIcon != null)
            dadBuffIcon.color = Color.white;

        isDadBuffActive = false;
    }

    IEnumerator MomBuffRoutine()
    {
        isMomBuffActive = true;

        // Activate buff
        if (playerVitalsManager != null)
            playerVitalsManager.SetInvincible(true);
        if (audioSource && momBuffSound)
            audioSource.PlayOneShot(momBuffSound);
        if (momBuffIcon != null)
            momBuffIcon.color = Color.white;
        if (momOutline != null)
        {
            momOutline.effectColor = Color.green;
            momOutline.enabled = true;
        }
        if (momActiveOverlay) momActiveOverlay.SetActive(true);

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
        if (playerVitalsManager != null)
            playerVitalsManager.SetInvincible(false);
        if (momOutline != null)
            momOutline.enabled = false;
        if (momBuffIcon != null)
            momBuffIcon.color = Color.grey;
        if (momDurationText) momDurationText.text = "";
        if (momActiveOverlay) momActiveOverlay.SetActive(false);
        if (momCooldownOverlay) momCooldownOverlay.SetActive(true);

        // Cooldown countdown
        float cooldownRemaining = momCooldown;
        while (cooldownRemaining > 0)
        {
            if (momCooldownText)
                momCooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }

        if (momCooldownText) momCooldownText.text = "";
        if (momCooldownOverlay) momCooldownOverlay.SetActive(false);
        if (momBuffIcon != null)
            momBuffIcon.color = Color.white;

        isMomBuffActive = false;
    }
}
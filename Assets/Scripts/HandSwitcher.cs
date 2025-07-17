using System;
using UnityEngine;
using UnityEngine.UI;
using WeaponType = PlayerStatsManager.WeaponType;

public class HandSwitcher : MonoBehaviour
{
    [Header("Hand Prefabs")]
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab;
    public GameObject hookAimHandPrefab;
    public GameObject gunHandPrefab;
    public GameObject gunAimHandPrefab;
    public GameObject runHandPrefab;
    public GameObject deadCameraHandPrefab;

    [Header("Death Effect")]
    public Image fadeToBlackImage;
    public float fadeDuration = 2f;
    public Transform playerCameraTransform;
    public Vector3 deathTiltRotation = new Vector3(-50f, 0f, 0f);
    public Vector3 deathFallOffset = new Vector3(0f, -0.3f, 0.3f);

    [Header("Audio")]
    public AudioClip deathImpactClip;

    [Header("Settings")]
    public float modeSwitchCooldown = 0.3f;

    public enum Mode { Idle = 0, Hook = 1, Gun = 2 }
    public static Mode CurrentMode = Mode.Idle;
    public static bool IsAiming { get; private set; }

    private bool isRunning = false;
    private bool isDead = false;
    private float sprintTimer = 0f;
    private float sprintDuration = 5f;
    private bool sprintCooldownActive = false;
    private float fadeTimer = 0f;
    private float lastModeSwitchTime;
    private Quaternion initialCameraRotation;
    private Quaternion targetCameraRotation;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;

    void Start()
    {
        if (playerCameraTransform != null)
        {
            initialCameraRotation = playerCameraTransform.rotation;
            initialCameraPosition = playerCameraTransform.position;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SwitchToDeadState();

        if (isDead)
        {
            HandleDeathEffects();
            return;
        }

        HandleHandSwitching();
        HandleAiming();
        HandleSprinting();
    }

    void HandleHandSwitching()
    {
        if (Time.time < lastModeSwitchTime + modeSwitchCooldown) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lastModeSwitchTime = Time.time;
            CurrentMode = (Mode)(((int)CurrentMode + 1) % 3);
            SetHandMode(CurrentMode);
        }
    }

    void HandleAiming()
    {
        if (CurrentMode == Mode.Hook || CurrentMode == Mode.Gun)
        {
            bool aimingInput = Input.GetMouseButton(1);
            if (aimingInput != IsAiming)
            {
                IsAiming = aimingInput;
                SetHandMode(CurrentMode);
            }
        }
        else if (IsAiming)
        {
            IsAiming = false;
            SetHandMode(CurrentMode);
        }
    }

    void HandleSprinting()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool forwardHeld = Input.GetKey(KeyCode.W);
        bool shouldSprint = shiftHeld && forwardHeld;

        if (shouldSprint && !isRunning && !sprintCooldownActive)
        {
            isRunning = true;
            sprintTimer = 0f;
            SetHandMode(CurrentMode);
        }

        if (isRunning)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= sprintDuration)
            {
                isRunning = false;
                sprintCooldownActive = true;
                SetHandMode(CurrentMode);
            }
        }

        if (!shiftHeld || !forwardHeld)
        {
            sprintCooldownActive = false;
            if (isRunning)
            {
                isRunning = false;
                SetHandMode(CurrentMode);
            }
        }
    }

    public void SwitchToDeadState()
    {
        if (isDead) return;

        isDead = true;
        DisableAllHands();

        if (deadCameraHandPrefab != null)
            deadCameraHandPrefab.SetActive(true);

        if (deathImpactClip != null && playerCameraTransform != null)
            AudioSource.PlayClipAtPoint(deathImpactClip, playerCameraTransform.position);

        if (fadeToBlackImage != null)
        {
            fadeToBlackImage.gameObject.SetActive(true);
            fadeToBlackImage.color = new Color(0, 0, 0, 0);
            fadeTimer = 0f;
        }

        if (playerCameraTransform != null)
        {
            initialCameraRotation = playerCameraTransform.rotation;
            targetCameraRotation = Quaternion.Euler(deathTiltRotation);
            initialCameraPosition = playerCameraTransform.position;
            targetCameraPosition = initialCameraPosition + deathFallOffset;
        }
    }

    void HandleDeathEffects()
    {
        if (playerCameraTransform == null) return;

        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        if (fadeToBlackImage != null)
            fadeToBlackImage.color = new Color(0f, 0f, 0f, t);

        playerCameraTransform.rotation = Quaternion.Slerp(
            initialCameraRotation, 
            targetCameraRotation, 
            t);
        playerCameraTransform.position = Vector3.Lerp(
            initialCameraPosition, 
            targetCameraPosition, 
            t);
    }

    public void SetHandMode(Mode mode)
    {
        DisableAllHands();

        if (isRunning && runHandPrefab != null)
        {
            runHandPrefab.SetActive(true);
            return;
        }

        switch (mode)
        {
            case Mode.Idle:
                idleHandPrefab?.SetActive(true);
                break;
            case Mode.Hook:
                if (IsAiming)
                    hookAimHandPrefab?.SetActive(true);
                else
                    hookHandPrefab?.SetActive(true);
                break;
            case Mode.Gun:
                if (IsAiming)
                    gunAimHandPrefab?.SetActive(true);
                else
                    gunHandPrefab?.SetActive(true);
                break;
        }

        if (PlayerStatsManager.Instance != null)
        {
            switch (mode)
            {
                case Mode.Idle:
                    PlayerStatsManager.Instance.SetWeapon(WeaponType.Unarmed);
                    break;
                case Mode.Hook:
                    PlayerStatsManager.Instance.SetWeapon(WeaponType.Hook);
                    break;
                case Mode.Gun:
                    PlayerStatsManager.Instance.SetWeapon(WeaponType.Gun);
                    if (WeaponManager.Instance != null)
                        WeaponManager.Instance.SetWeapon(0);
                    break;
            }
        }
    }

    void DisableAllHands()
    {
        idleHandPrefab?.SetActive(false);
        hookHandPrefab?.SetActive(false);
        hookAimHandPrefab?.SetActive(false);
        gunHandPrefab?.SetActive(false);
        gunAimHandPrefab?.SetActive(false);
        runHandPrefab?.SetActive(false);
        deadCameraHandPrefab?.SetActive(false);
    }
}
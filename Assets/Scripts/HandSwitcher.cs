using System;
using UnityEngine;
using UnityEngine.UI;

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
    public Image fadeToBlackImage;                // Fullscreen UI image (black)
    public float fadeDuration = 2f;               // Fade time
    public Transform playerCameraTransform;       // Drag your main camera here
    public Vector3 deathTiltRotation = new Vector3(-50f, 0f, 0f);
    public Vector3 deathFallOffset = new Vector3(0f, -0.3f, 0.3f);

    public enum Mode { Idle = 0, Hook = 1, Gun = 2 }
    public static Mode CurrentMode = Mode.Idle;
    public static bool IsAiming { get; private set; }

    private bool isRunning = false;
    private bool isDead = false;

    private float sprintTimer = 0f;
    private float sprintDuration = 5f;
    private bool sprintCooldownActive = false;

    private float fadeTimer = 0f;
    private Quaternion initialCameraRotation;
    private Quaternion targetCameraRotation;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;

    [Header("Gun Shoot Points")]
    public Transform gunHandShootPoint;
    public Transform gunAimHandShootPoint;
    public GunShooter gunShooter;  // Assign this via Inspector


    [Header("Audio")]
    public AudioClip deathImpactClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Test: press K to die
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
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
        isDead = true;
        DisableAllHands();

        if (deadCameraHandPrefab != null)
            deadCameraHandPrefab.SetActive(true);

        // 🔊 Play death sound
        if (deathImpactClip != null)
        {
            AudioSource.PlayClipAtPoint(deathImpactClip, playerCameraTransform.position);
        }

        // Fade to black setup
        if (fadeToBlackImage != null)
        {
            fadeToBlackImage.gameObject.SetActive(true);
            fadeToBlackImage.color = new Color(0, 0, 0, 0);
            fadeTimer = 0f;
        }

        // Tilt & fall camera setup
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
        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        if (fadeToBlackImage != null)
        {
            fadeToBlackImage.color = new Color(0f, 0f, 0f, t);
        }

        if (playerCameraTransform != null)
        {
            playerCameraTransform.rotation = Quaternion.Slerp(initialCameraRotation, targetCameraRotation, t);
            playerCameraTransform.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, t);
        }
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
                    {
                        gunAimHandPrefab?.SetActive(true);
                        if (gunShooter != null)
                        {
                            gunShooter.firePoint = gunAimHandShootPoint;
                            gunShooter.isActiveShooter = gunAimHandPrefab.activeSelf; // Activate only this shooter
                        }
                    }
                    else
                    {
                        gunHandPrefab?.SetActive(true);
                        if (gunShooter != null)
                        {
                            gunShooter.firePoint = gunHandShootPoint;
                            gunShooter.isActiveShooter = gunHandPrefab.activeSelf; // Activate only this shooter
                        }
                    }
                    break;

        }

                // Tell WeaponManager to sync icon based on mode
        if (WeaponManager.Instance != null)
        {
            switch (mode)
            {
                case Mode.Idle:
                    WeaponManager.Instance.SetWeapon(WeaponManager.WeaponType.Unarmed);
                    break;
                case Mode.Hook:
                    WeaponManager.Instance.SetWeapon(WeaponManager.WeaponType.Hook);
                    break;
                case Mode.Gun:
                    WeaponManager.Instance.SetWeapon(WeaponManager.WeaponType.Gun);
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

        if (gunShooter != null)
        {
            gunShooter.isActiveShooter = false; // Disable any shooting during switching
        }
    }
}

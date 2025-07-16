using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab;
    public GameObject hookAimHandPrefab;
    public GameObject gunHandPrefab;
    public GameObject gunAimHandPrefab;
    public GameObject runHandPrefab;
    public GameObject deadCameraHandPrefab;

    public static int CurrentMode = 0; // 0 = idle, 1 = hook, 2 = gun
    public static bool IsAiming { get; private set; }

    private bool isRunning = false;
    private bool isDead = false;

    private float sprintTimer = 0f;
    private float sprintDuration = 5f;
    private bool sprintCooldownActive = false;

    void Update()
    {
        if (isDead) return;

        // Hand switching
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentMode = (CurrentMode + 1) % 3; // Idle, Hook, Gun
            SetHandMode(CurrentMode);
        }

        // Aiming toggle
        if (CurrentMode == 1 || CurrentMode == 2)
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

        // Sprinting logic
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool forwardHeld = Input.GetKey(KeyCode.W);
        bool shouldSprint = shiftHeld && forwardHeld;

        if (shouldSprint && !isRunning && !sprintCooldownActive)
        {
            // Start sprint
            isRunning = true;
            sprintTimer = 0f;
            SetHandMode(CurrentMode);
        }

        if (isRunning)
        {
            sprintTimer += Time.deltaTime;

            if (sprintTimer >= sprintDuration)
            {
                // Sprint ends after 5 seconds
                isRunning = false;
                sprintCooldownActive = true;
                SetHandMode(CurrentMode);
            }
        }

        // Reset sprint cooldown when player releases Shift or W
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
    }

    public void SetHandMode(int mode)
    {
        DisableAllHands();

        if (isRunning && runHandPrefab != null)
        {
            runHandPrefab.SetActive(true);
            return;
        }

        switch (mode)
        {
            case 0: idleHandPrefab?.SetActive(true); break;
            case 1:
                if (IsAiming && hookAimHandPrefab != null)
                    hookAimHandPrefab.SetActive(true);
                else
                    hookHandPrefab?.SetActive(true);
                break;
            case 2:
                if (IsAiming && gunAimHandPrefab != null)
                    gunAimHandPrefab.SetActive(true);
                else
                    gunHandPrefab?.SetActive(true);
                break;
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

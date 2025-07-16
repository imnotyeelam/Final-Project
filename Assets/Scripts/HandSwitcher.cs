using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    [Header("Hand Prefabs")]
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab;
    public GameObject hookAimHandPrefab; 
    public GameObject fightHandPrefab;
    public GameObject gunHandPrefab;
    public GameObject gunAimHandPrefab;
    public GameObject runHandPrefab;

    public static int CurrentMode = 0; // 0=Idle, 1=Hook, 2=Fight, 3=Gun
    public static bool IsAiming { get; private set; }

    private bool isRunning = false;

    void Update()
    {
        // Mode switching with Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentMode = (CurrentMode + 1) % 4;
            SetHandMode(CurrentMode);
        }

        // Aiming logic for both gun and hook modes
        if (CurrentMode == 1 || CurrentMode == 3) // Hook or Gun mode
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

        // Running logic
        bool runPressed = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        if (runPressed != isRunning)
        {
            isRunning = runPressed;
            SetHandMode(CurrentMode);
        }
    }

    void SetHandMode(int mode)
    {
        // Running takes priority
        if (isRunning)
        {
            DisableAllHands();
            if (runHandPrefab != null) runHandPrefab.SetActive(true);
            return;
        }

        // Handle regular modes
        idleHandPrefab.SetActive(mode == 0);
        fightHandPrefab.SetActive(mode == 2);
        
        // Hook mode with aiming
        if (mode == 1)
        {
            hookHandPrefab.SetActive(!IsAiming);
            if (hookAimHandPrefab != null) hookAimHandPrefab.SetActive(IsAiming);
        }
        
        // Gun mode with aiming
        if (mode == 3)
        {
            gunHandPrefab.SetActive(!IsAiming);
            if (gunAimHandPrefab != null) gunAimHandPrefab.SetActive(IsAiming);
        }
        
        if (runHandPrefab != null) runHandPrefab.SetActive(false);
    }

    void DisableAllHands()
    {
        idleHandPrefab.SetActive(false);
        hookHandPrefab.SetActive(false);
        if (hookAimHandPrefab != null) hookAimHandPrefab.SetActive(false);
        fightHandPrefab.SetActive(false);
        gunHandPrefab.SetActive(false);
        if (gunAimHandPrefab != null) gunAimHandPrefab.SetActive(false);
        if (runHandPrefab != null) runHandPrefab.SetActive(false);
    }
}
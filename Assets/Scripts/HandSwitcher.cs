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
        // First disable ALL hands
        DisableAllHands();
        
        // If running, show run hand and exit
        if (isRunning && runHandPrefab != null)
        {
            runHandPrefab.SetActive(true);
            return;
        }

        // Enable the correct hand based on mode
        switch(mode)
        {
            case 0: // Idle
                idleHandPrefab.SetActive(true);
                break;
            case 1: // Hook
                if (IsAiming && hookAimHandPrefab != null)
                    hookAimHandPrefab.SetActive(true);
                else
                    hookHandPrefab.SetActive(true);
                break;
            case 2: // Fight
                fightHandPrefab.SetActive(true);
                break;
            case 3: // Gun
                if (IsAiming && gunAimHandPrefab != null)
                    gunAimHandPrefab.SetActive(true);
                else
                    gunHandPrefab.SetActive(true);
                break;
        }
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
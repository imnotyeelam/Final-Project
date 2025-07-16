using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab;
    public GameObject hookAimHandPrefab;
    public GameObject fightHandPrefab;
    public GameObject gunHandPrefab;
    public GameObject gunAimHandPrefab;
    public GameObject runHandPrefab;
    public GameObject deadCameraHandPrefab;

    public static int CurrentMode = 0;
    public static bool IsAiming { get; private set; }

    private bool isRunning = false;
    private bool isDead = false;

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentMode = (CurrentMode + 1) % 4;
            SetHandMode(CurrentMode);
        }

        if (CurrentMode == 1 || CurrentMode == 3)
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

        bool runPressed = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        if (runPressed != isRunning)
        {
            isRunning = runPressed;
            SetHandMode(CurrentMode);
        }
    }

    public void SwitchToDeadState()
    {
        isDead = true;
        DisableAllHands();

        if (deadCameraHandPrefab != null)
            deadCameraHandPrefab.SetActive(true);
    }

    void SetHandMode(int mode)
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
            case 2: fightHandPrefab?.SetActive(true); break;
            case 3:
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
        fightHandPrefab?.SetActive(false);
        gunHandPrefab?.SetActive(false);
        gunAimHandPrefab?.SetActive(false);
        runHandPrefab?.SetActive(false);
        deadCameraHandPrefab?.SetActive(false);
    }
}

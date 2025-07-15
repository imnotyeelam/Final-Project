using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab; 
    public GameObject fightHandPrefab;
    public GameObject gunHandPrefab;
    public GameObject runHandPrefab;

    public static int CurrentMode = 0; // 0 = Idle, 1 = Hook, 2 = Fight, 3 = Gun

    private bool isRunning = false;

    void Start()
    {
        SetHandMode(CurrentMode);
    }

    void Update()
    {
        // Switch modes with Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentMode = (CurrentMode + 1) % 4;
            SetHandMode(CurrentMode);
        }

        // Detect running input (Shift + W)
        bool runPressed = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        if (runPressed != isRunning)
        {
            isRunning = runPressed;
            SetHandMode(CurrentMode);
        }
    }

    void SetHandMode(int mode)
    {
        // Use running hand if running
        if (isRunning)
        {
            DisableAllHands();
            if (runHandPrefab != null) runHandPrefab.SetActive(true);
            return;
        }

        // Otherwise show based on current mode
        idleHandPrefab.SetActive(mode == 0);
        hookHandPrefab.SetActive(mode == 1);  
        fightHandPrefab.SetActive(mode == 2);
        gunHandPrefab.SetActive(mode == 3);
        if (runHandPrefab != null) runHandPrefab.SetActive(false);
    }

    void DisableAllHands()
    {
        idleHandPrefab.SetActive(false);
        hookHandPrefab.SetActive(false); 
        fightHandPrefab.SetActive(false);
        gunHandPrefab.SetActive(false);
        if (runHandPrefab != null) runHandPrefab.SetActive(false);
    }
}

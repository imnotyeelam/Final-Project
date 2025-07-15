using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject idleHandPrefab;
    public GameObject hookHandPrefab; // renamed from fistHandPrefab
    public GameObject fightHandPrefab;
    public GameObject gunHandPrefab;

    // 0 = Idle, 1 = Hook (used for grappling), 2 = Fight, 3 = Gun
    public static int CurrentMode { get; private set; } = 0;

    void Start()
    {
        SetHandMode(CurrentMode);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentMode = (CurrentMode + 1) % 4; // 0 → 1 → 2 → 3 → 0 ...
            SetHandMode(CurrentMode);
        }
    }

    void SetHandMode(int mode)
    {
        idleHandPrefab.SetActive(mode == 0);
        hookHandPrefab.SetActive(mode == 1);
        fightHandPrefab.SetActive(mode == 2);
        gunHandPrefab.SetActive(mode == 3);
    }
}

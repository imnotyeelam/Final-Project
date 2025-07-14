using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject idleHandPrefab;
    public GameObject fistHandPrefab;
    public GameObject fightHandPrefab;
    public GameObject gunHandPrefab;

    private int currentMode = 0; // 0 = Idle, 1 = Fist, 2 = Fight, 3 = Gun

    void Start()
    {
        SetHandMode(currentMode);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMode = (currentMode + 1) % 4; // Cycle through 0 → 1 → 2 → 3 → 0 ...
            SetHandMode(currentMode);
        }
    }

    void SetHandMode(int mode)
    {
        idleHandPrefab.SetActive(mode == 0);
        fistHandPrefab.SetActive(mode == 1);
        fightHandPrefab.SetActive(mode == 2);
        gunHandPrefab.SetActive(mode == 3);
    }
}

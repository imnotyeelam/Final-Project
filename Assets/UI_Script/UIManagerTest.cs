using UnityEngine;

public class UIManagerTest : MonoBehaviour
{
    private float currentHP = 100f;
    private float maxHP = 100f;

    private float currentEnergy = 100f;
    private float maxEnergy = 100f;

    private int collectedPieces = 0;
    private int totalPieces = 3;

    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHP, maxHP);
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
            UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
        }

        if (UIManager.Instance != null && WeaponManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmoUI(
                WeaponManager.Instance.currentAmmo,
                WeaponManager.Instance.maxAmmo
            );
        }
    }

    void Update()
    {
        // --- HP ---
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHP = Mathf.Max(0, currentHP - 10);
            UIManager.Instance.UpdateHealth(currentHP, maxHP);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentHP = Mathf.Min(maxHP, currentHP + 10);
            UIManager.Instance.UpdateHealth(currentHP, maxHP);
        }

        // --- Energy ---
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentEnergy = Mathf.Max(0, currentEnergy - 5);
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + 5);
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        }

        // --- Pieces ---
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (collectedPieces < totalPieces)
            {
                collectedPieces++;
                UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
            }
        }

        // --- Simulate props pickup ---
        if (Input.GetKeyDown(KeyCode.O)) UIManager.Instance.AddProp("HP");
        if (Input.GetKeyDown(KeyCode.I)) UIManager.Instance.AddProp("Ammo");
        if (Input.GetKeyDown(KeyCode.P)) UIManager.Instance.AddProp("Energy");

        // --- Use props ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) // use ammo prop
        {
            if (UIManager.Instance.UseProp("Ammo"))
            {
                WeaponManager.Instance.AddAmmo(10);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // use HP prop
        {
            if (UIManager.Instance.UseProp("HP"))
            {
                currentHP = Mathf.Min(maxHP, currentHP + 10);
                UIManager.Instance.UpdateHealth(currentHP, maxHP);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // use Energy prop
        {
            if (UIManager.Instance.UseProp("Energy"))
            {
                currentEnergy = Mathf.Min(maxEnergy, currentEnergy + 10);
                UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
            }
        }

        // --- Simulate shooting ---
        if (Input.GetMouseButtonDown(0))
        {
            WeaponManager.Instance.Shoot();
        }
    }
}

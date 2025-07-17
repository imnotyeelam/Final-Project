using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeaponIndex = 0;

    public static WeaponManager Instance { get; private set; } // Changed from object to WeaponManager

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentWeaponIndex = index;
        UIManager.Instance?.UpdateWeapon(GetCurrentWeaponName());
    }

    public string GetCurrentWeaponName()
    {
        if (currentWeaponIndex < 0 || currentWeaponIndex >= weapons.Length)
            return "No Weapon";
            
        GameObject currentWeapon = weapons[currentWeaponIndex];
        return currentWeapon != null ? currentWeapon.name : "No Weapon";
    }

    public void SetWeapon(int index)
    {
        EquipWeapon(index);
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }

    public void AddAmmo(int amount)
    {
        // Implement weapon-specific ammo logic here if needed
        PlayerStatsManager.Instance?.AddAmmo(amount);
    }
}
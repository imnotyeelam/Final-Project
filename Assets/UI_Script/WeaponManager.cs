using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public enum WeaponType { Idle, Hook, Gun }

    public GameObject currentWeapon;
    public GameObject weaponHolder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        if (currentWeapon != null)
            EquipWeapon(currentWeapon);
    }

    // Overload for convenience
    public void EquipWeapon(GameObject weaponPrefab)
    {
        EquipWeapon(weaponPrefab.name, weaponPrefab);
    }

    public void EquipWeapon(string weaponName, GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(weaponPrefab, weaponHolder.transform);
        currentWeapon.name = weaponName;

        Debug.Log("Equipped weapon: " + weaponName);
    }

    public void UnequipWeapon()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = null;

        UIManager.Instance?.ammoPanel?.SetActive(false);
    }

    public void RefillAmmo()
    {
        if (currentWeapon == null) return;

        GunAmmo ammo = currentWeapon.GetComponent<GunAmmo>();
        if (ammo == null)
            ammo = currentWeapon.GetComponentInChildren<GunAmmo>();

        if (ammo != null)
        {
            ammo.Refill();
            UIManager.Instance?.AddTask("Ammo refilled!");
        }
        else
        {
            Debug.LogWarning("GunAmmo script not found on current weapon.");
        }
    }

    public void SetWeapon(WeaponType type)
    {
        Debug.Log($"HandSwitcher set to: {type}");
    }
}

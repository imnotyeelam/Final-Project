using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public enum WeaponType { Unarmed, Hook, Gun }
    public WeaponType currentWeapon = WeaponType.Unarmed;

    [Header("Weapon UI")]
    public Image weaponIcon;       // Current weapon icon (will change sprite)
    public Sprite unarmedSprite;   // Unarmed icon
    public Sprite hookSprite;      // Hook icon
    public Sprite gunSprite;       // Gun icon

    [Header("Ammo UI")]
    public GameObject ammoPanel;   // Only visible when gun is equipped
    public Text ammoText;          // Displays ammo count

    [Header("Gun Settings")]
    public int currentAmmo = 10;
    public int maxAmmo = 30;

    void Start()
    {
        UpdateWeaponUI(); // Initialize UI
    }

    void Update()
    {
        // Press Q to switch weapons
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWeapon();
        }

        // For testing: Left mouse button to shoot
        if (currentWeapon == WeaponType.Gun && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void SwitchWeapon()
    {
        // Cycle through weapons
        if (currentWeapon == WeaponType.Unarmed) currentWeapon = WeaponType.Hook;
        else if (currentWeapon == WeaponType.Hook) currentWeapon = WeaponType.Gun;
        else currentWeapon = WeaponType.Unarmed;

        Debug.Log("Switched to: " + currentWeapon);
        UpdateWeaponUI();
    }

    void UpdateWeaponUI()
    {
        if (!weaponIcon) return;

        // Change icon based on current weapon
        switch (currentWeapon)
        {
            case WeaponType.Unarmed:
                weaponIcon.sprite = unarmedSprite;
                ammoPanel.SetActive(false);
                break;

            case WeaponType.Hook:
                weaponIcon.sprite = hookSprite;
                ammoPanel.SetActive(false);
                break;

            case WeaponType.Gun:
                weaponIcon.sprite = gunSprite;
                ammoPanel.SetActive(true);
                UpdateAmmoUI();
                break;
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText)
        {
            ammoText.text = $"{currentAmmo}";
        }
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            Debug.Log("Bang! Ammo left: " + currentAmmo);

            //check after shooting
            if (currentAmmo == 0)
            {
                UIManager.Instance.ShowOutOfAmmo(true);
            }
        }
        else
        {
            Debug.Log("No ammo!");
            UIManager.Instance.ShowOutOfAmmo(true);
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log("Ammo reloaded: " + currentAmmo);

        //if ammo > 0, hide warning
        if (currentAmmo > 0)
        {
            UIManager.Instance.ShowOutOfAmmo(false);
        }
    }
}
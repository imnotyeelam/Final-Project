using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Debug.LogWarning("Duplicate WeaponManager found, destroying extra one.");
            Destroy(gameObject);
        }
    }

    public enum WeaponType { Unarmed, Hook, Gun }
    public WeaponType currentWeapon = WeaponType.Unarmed;

    [Header("Weapon UI")]
    public Image weaponIcon;       // Current weapon icon (will change sprite)
    public Sprite unarmedSprite;   // Unarmed icon
    public Sprite hookSprite;      // Hook icon
    public Sprite gunSprite;       // Gun icon


    [Header("Gun Settings")]
    public int currentAmmo = 10;
    public int maxAmmo = 30;

    [Tooltip("How many bullets to decrease per shot")]
    public int ammoPerShot = 1; 
    void Start()
    {
        UpdateWeaponUI(); // Initialize UI
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWeapon();
        }

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

        switch (currentWeapon)
        {
            case WeaponType.Unarmed:
                weaponIcon.sprite = unarmedSprite;
                UIManager.Instance.ammoPanel.SetActive(false);
                break;

            case WeaponType.Hook:
                weaponIcon.sprite = hookSprite;
                UIManager.Instance.ammoPanel.SetActive(false);
                break;

            case WeaponType.Gun:
                weaponIcon.sprite = gunSprite;
                UIManager.Instance.ammoPanel.SetActive(true);
                UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo); 
                break;
        }
    }
    public void Shoot()
    {
        Debug.Log($"[Shoot] called at frame: {Time.frameCount}, from: {gameObject.name}");

        if (currentWeapon != WeaponType.Gun)
        {
            Debug.LogWarning("Tried to shoot, but weapon is not Gun.");
            return;
        }

        if (currentAmmo >= ammoPerShot)
        {
            currentAmmo -= ammoPerShot;
            UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
            Debug.Log("Bang! Ammo left: " + currentAmmo);

            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                UIManager.Instance.ShowOutOfAmmo(true);
            }
        }
        else
        {
            Debug.Log("Not enough ammo to fire!");
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
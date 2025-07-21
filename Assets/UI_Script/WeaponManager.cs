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
    public Image weaponIcon;
    public Sprite unarmedSprite;
    public Sprite hookSprite;
    public Sprite gunSprite;

    [Header("Gun Settings")]
    public int currentAmmo;
    public int maxAmmo = 60;
    public int ammoPerShot = 1;

    void Start()
    {
        UpdateWeaponUI();
    }

    void Update()
    {
        if (currentWeapon == WeaponType.Gun && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void SetWeapon(WeaponType newWeapon)
    {
        if (currentWeapon == newWeapon) return;
        currentWeapon = newWeapon;
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
        if (currentWeapon != WeaponType.Gun) return;

        if (currentAmmo >= ammoPerShot)
        {
            currentAmmo -= ammoPerShot;
            UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);

            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                UIManager.Instance.ShowOutOfAmmo(true);
            }
        }
        else
        {
            UIManager.Instance.ShowOutOfAmmo(true);
        }
    }

    [System.Obsolete]
    public void AddAmmo(int amount)
    {
        if (currentAmmo >= maxAmmo)
        {
            UIManager.Instance.ShowPrompt("Ammo is already full!");
            return;
        }

        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);

        if (currentAmmo > 0)
            UIManager.Instance.ShowOutOfAmmo(false);
    }
}

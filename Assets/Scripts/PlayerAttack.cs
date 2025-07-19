using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator handAnimator;
    public float attackMultiplier = 1f; // <-- added multiplier field
    public HandSwitcher handSwitcher;
    public GunShooter gunShooter;

    public int GetCurrentAmmo()
    {
        return gunShooter.GetCurrentAmmo();
    }


    void Start()
    {
        gunShooter = GunShooter.Instance;  
    }

    [System.Obsolete]
    void Update()
    {
        // Only allow shooting when in gun-hand mode
    if (HandSwitcher.CurrentMode == HandSwitcher.Mode.Gun && Input.GetMouseButtonDown(0))
        {
            gunShooter.TryShoot();
        }
    }

    // Call this from HandSwitcher when switching to Gun Hand
    public void SyncAmmoUI()
    {
        if (gunShooter != null)
        {
            int currentAmmo = gunShooter.GetCurrentAmmo();
            int maxAmmo = gunShooter.maxAmmo;
            UIManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
        }
    }
}

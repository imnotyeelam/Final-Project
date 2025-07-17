using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void RefillAmmo()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Ammo refilled to max.");
    }

    public void UseAmmo(int amount)
    {
        currentAmmo -= amount;
        if (currentAmmo < 0) currentAmmo = 0;
    }
}

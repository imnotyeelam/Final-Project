using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Refill()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Ammo refilled to max.");
    }
}

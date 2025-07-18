using UnityEngine;

public class GunInput : MonoBehaviour
{
    public GunShooter gunShooter;

    [System.Obsolete]
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (gunShooter != null)
                gunShooter.TryShoot();
            else
                Debug.LogWarning("GunShooter reference is missing on " + gameObject.name);
        }
    }
}

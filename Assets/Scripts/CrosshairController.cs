using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshairUI;
    public Aiming gunAiming;

    void Update()
    {
        if (crosshairUI != null && gunAiming != null)
        {
            crosshairUI.SetActive(!gunAiming.IsAiming());
        }
    }
}

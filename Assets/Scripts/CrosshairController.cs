using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshairUI;       // assign your crosshair image in Inspector
    public Aiming aimingScript;          // drag the gun (which has the Aiming.cs) here

    void Update()
    {
        if (crosshairUI != null && aimingScript != null)
        {
            crosshairUI.SetActive(!aimingScript.IsAiming());
        }
    }
}

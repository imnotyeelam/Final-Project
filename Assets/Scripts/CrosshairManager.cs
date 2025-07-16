using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshairUI;
    public Color normalColor = Color.white;
    public Color aimColor = Color.red;

    void Update()
    {
        if (crosshairUI == null) return;

        bool shouldShow = HandSwitcher.CurrentMode == 3 && HandSwitcher.IsAiming;
        crosshairUI.SetActive(shouldShow);
        Debug.Log("Crosshair visible? " + shouldShow);
    }
}

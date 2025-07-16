using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshairUI;
    public Color normalColor = Color.white;
    public Color aimColor = Color.red; // Color change when aiming

    void Update()
    {
        if (crosshairUI == null) return;

        // Show crosshair only in gun mode
        bool shouldShow = HandSwitcher.CurrentMode == 3;
        crosshairUI.SetActive(shouldShow);

    }
}
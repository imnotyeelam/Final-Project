using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public GameObject crosshairUI;
    public Color normalColor = Color.white;
    public Color aimColor = Color.red; // Color change when aiming

    void Update()
    {
        if (crosshairUI == null) return;

        // Show crosshair only in gun or hook modes
        bool shouldShow = HandSwitcher.CurrentMode == 1 || HandSwitcher.CurrentMode == 3;
        crosshairUI.SetActive(shouldShow);

        // Change color when aiming
        if (shouldShow)
        {
            var image = crosshairUI.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = HandSwitcher.IsAiming ? aimColor : normalColor;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage; // Assign your crosshair UI Image here
    public Color normalColor = Color.white;
    public Color aimColor = Color.red; // Color when aiming down sights
    
    void Update()
    {
        if (crosshairImage == null) return;
        
        // Always show crosshair in gun mode (mode 3)
        bool shouldShow = HandSwitcher.CurrentMode == 3 && HandSwitcher.IsAiming;
        crosshairImage.enabled = shouldShow;
        
        if (shouldShow)
        {
            // Change color based on aiming state
            crosshairImage.color = HandSwitcher.IsAiming ? aimColor : normalColor;
            
            // Optional: Scale effect when aiming
            float scale = HandSwitcher.IsAiming ? 0.8f : 1f;
            crosshairImage.transform.localScale = Vector3.one * scale;
        }
    }
}
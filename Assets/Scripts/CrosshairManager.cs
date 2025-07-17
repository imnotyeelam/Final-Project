using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public RawImage crosshairImage; 
    public Color gunAimColor = Color.red;
    public Color defaultColor = Color.white;

    void Start()
    {
        if (crosshairImage != null)
        {
            crosshairImage.enabled = false;
        }
    }

    void Update()
    {
        if (crosshairImage == null) return;

        // ✅ Only show crosshair when aiming in GUN mode
        bool shouldShow = HandSwitcher.CurrentMode == HandSwitcher.Mode.Gun && HandSwitcher.IsAiming;

        crosshairImage.enabled = shouldShow;

        if (shouldShow)
        {
            crosshairImage.color = gunAimColor;
            crosshairImage.transform.localScale = Vector3.one * 0.8f;
        }
    }

    void OnDisable()
    {
        if (crosshairImage != null)
        {
            crosshairImage.enabled = false;
        }
    }
}

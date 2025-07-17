using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.0f;

    private bool isFading = false;
    private float targetAlpha = 0f;

    void Start()
    {
        if (fadeImage != null)
        {
            // Start fully transparent
            SetAlpha(0f);
        }
    }

    void Update()
    {
        if (isFading && fadeImage != null)
        {
            Color currentColor = fadeImage.color;
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            SetAlpha(newAlpha);

            if (Mathf.Approximately(newAlpha, targetAlpha))
                isFading = false;
        }
    }

    public void FadeToBlack()
    {
        targetAlpha = 1f;
        isFading = true;
    }

    public void FadeToClear()
    {
        targetAlpha = 0f;
        isFading = true;
    }

    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TaskPanelToggle : MonoBehaviour
{
    [Header("Panel Animation Settings")]
    public RectTransform taskPanel;
    public Vector2 shownPos;
    public Vector2 hiddenPos;

    [Header("Scroll Settings")]
    public ScrollRect scrollRect;
    public Scrollbar verticalScrollbar;
    public Scrollbar horizontalScrollbar;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip toggleClip;

    private bool isShown = true;

    [Header("Scroll Settings")]
    public float scrollSpeed = 3.0f; // adjust this to change scroll sensitivity

    void Start()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar = verticalScrollbar;
            scrollRect.horizontalScrollbar = horizontalScrollbar;
        }
    }

    void Update()
    {
        // Toggle Panel
        if (Input.GetKeyDown(KeyCode.T))
        {
            isShown = !isShown;

            // Play sound
            if (audioSource != null && toggleClip != null)
            {
                audioSource.PlayOneShot(toggleClip);
            }

            taskPanel.DOAnchorPos(isShown ? shownPos : hiddenPos, 0.3f)
                    .SetEase(Ease.OutCubic);
        }

        // Scroll with arrow keys
        if (scrollRect != null && isShown)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                scrollRect.verticalNormalizedPosition += scrollSpeed * Time.deltaTime;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
            }
        }
    }

    public void ScrollToLatestTask()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}

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
    public float scrollSpeed = 1000.0f; // Increased from 500 to 1000 for faster scrolling
    public float scrollAcceleration = 2.0f; // Added acceleration for faster scrolling when holding keys


    private float currentScrollSpeed;


    void Start()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar = verticalScrollbar;
            scrollRect.horizontalScrollbar = horizontalScrollbar;
        }
        currentScrollSpeed = scrollSpeed;
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
            // Accelerate scroll speed if key is held down
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2))
            {
                currentScrollSpeed += scrollAcceleration * Time.deltaTime;
                currentScrollSpeed = Mathf.Min(currentScrollSpeed, scrollSpeed * 3); // Cap at 3x normal speed
            }
            else
            {
                currentScrollSpeed = scrollSpeed; // Reset to base speed when no keys are pressed
            }


            float scrollDelta = currentScrollSpeed * Time.deltaTime;


            if (Input.GetKey(KeyCode.Alpha2))
            {
                scrollRect.verticalNormalizedPosition -= scrollDelta / scrollRect.content.sizeDelta.y;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
            }
            else if (Input.GetKey(KeyCode.Alpha1))
            {
                scrollRect.verticalNormalizedPosition += scrollDelta / scrollRect.content.sizeDelta.y;
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

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

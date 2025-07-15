using UnityEngine;
using DG.Tweening;

public class TaskPanelToggle : MonoBehaviour
{
    public RectTransform taskPanel;
    public Vector2 shownPos;
    public Vector2 hiddenPos;

    private bool isShown = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isShown = !isShown;
            taskPanel.DOAnchorPos(isShown ? shownPos : hiddenPos, 0.3f)
                     .SetEase(Ease.OutCubic);
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class TaskPanelGroup : MonoBehaviour
{
    public RectTransform taskPanel; 
    public Vector2 shownPos;
    public Vector2 hiddenPos; 
    private bool isShown = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit object: " + hit.collider.name);

                if (hit.collider.CompareTag("TaskToggleButton"))
                {
                    Debug.Log("Toggle Button clicked!");
                    TogglePanel();
                }
            }
        }
    }

    void TogglePanel()
    {
        isShown = !isShown;
        taskPanel.DOAnchorPos(isShown ? shownPos : hiddenPos, 0.5f).SetEase(Ease.OutCubic);
    }
}

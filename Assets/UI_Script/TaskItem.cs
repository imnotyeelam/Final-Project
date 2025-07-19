using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public Image checkbox;

    public bool IsCompleted { get; private set; }

    public void Setup(string taskName)
    {
        if (descriptionText != null)
        {
            descriptionText.text = taskName;
            Debug.Log("Set task text: " + taskName); // Debug log
        }
        else
        {
            Debug.LogError("descriptionText is not assigned in TaskItem.");
        }
    }


    public void MarkCompleted()
    {
        IsCompleted = true;

        if (checkbox)
            checkbox.enabled = true;

        Destroy(gameObject, 1f); // Optional delay before removal
    }
}

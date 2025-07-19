using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Toggle checkbox;
    public Text descriptionText;

    private string taskDescription;
    public bool isCompleted = false;

    public void Setup(string description)
    {
        Debug.Log("Setting up task: " + description);
        taskDescription = description;

        if (descriptionText != null)
            descriptionText.text = taskDescription;

        if (checkbox != null)
            checkbox.isOn = isCompleted;
    }

    public void MarkCompleted()
    {
        isCompleted = true;

        if (checkbox != null)
            checkbox.isOn = true;

        if (descriptionText != null)
            descriptionText.color = Color.gray;
    }

    public string GetDescription()
    {
        return taskDescription;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}

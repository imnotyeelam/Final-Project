using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Text descriptionText;

    private string taskDescription;

    public void Setup(string description)
    {
        taskDescription = description;

        if (descriptionText != null)
            descriptionText.text = taskDescription;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Text descriptionText;
    public Toggle checkbox;

    public bool isCompleted = false;

    public void Setup(string description)
    {
        Debug.Log("Set task text: " + description);
        descriptionText.text = description;
        checkbox.isOn = false;
        isCompleted = false;
    }

    public void MarkCompleted()
    {
        isCompleted = true;
        checkbox.isOn = true;
        descriptionText.color = Color.gray; // You can change this to Color.green if you prefer
    }

    public string GetDescription()
    {
        return descriptionText.text;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}

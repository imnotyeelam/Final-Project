using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    void Start()
    {
        // Add all tasks at the beginning
        AddGameTask("Collect HP Prop");
        AddGameTask("Kill Enemy");
        AddGameTask("Talk to NPC");
    }

    public void AddGameTask(string taskDescription)
    {
        Debug.Log("Creating task: " + taskDescription + " ✅ from GameTaskManager");

        if (UIManager.Instance != null)
        {
            var item = UIManager.Instance.AddTask(taskDescription);
            if (item != null)
            {
                Debug.Log("✅ Task created: " + taskDescription);
            }
            else
            {
                Debug.LogError("❌ Task creation failed in UIManager.");
            }
        }
        else
        {
            Debug.LogError("❌ UIManager.Instance is null!");
        }
    }

    public void CompleteTask(string taskDescription)
    {
        foreach (var task in UIManager.taskList)
        {
            if (task.GetDescription() == taskDescription && !task.IsCompleted())
            {
                task.MarkCompleted();
                return;
            }
        }
        Debug.LogWarning("⚠️ Task not found or already completed: " + taskDescription);
    }
}

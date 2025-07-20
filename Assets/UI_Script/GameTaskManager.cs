using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    void Start()
    {
        // Just show tasks as instructions
        AddGameTask("Collect HP Prop");
        AddGameTask("Kill Enemy");
        AddGameTask("Talk to NPC");
    }

    public void AddGameTask(string taskDescription)
    {
        Debug.Log("Adding task: " + taskDescription);

        if (UIManager.Instance != null)
        {
            var item = UIManager.Instance.AddTask(taskDescription);
            if (item != null)
            {
                Debug.Log("✅ Task displayed: " + taskDescription);
            }
            else
            {
                Debug.LogError("❌ Failed to create task UI!");
            }
        }
        else
        {
            Debug.LogError("❌ UIManager.Instance is null!");
        }
    }
}

using UnityEngine;


public class GameTaskManager : MonoBehaviour
{
    void Start()
    {
        // Show actual game tasks
        AddGameTask("Talk to NPC");
        AddGameTask("Go into bathroom");
    }


    public void AddGameTask(string taskDescription)
    {
        if (UIManager.Instance != null)
        {
            var item = UIManager.Instance.AddTask(taskDescription);
            if (item == null)
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

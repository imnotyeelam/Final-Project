using UnityEngine;
using System.Collections.Generic;

public class UITest : MonoBehaviour
{
    private List<GameObject> taskList = new List<GameObject>();
    private bool tasksCompleted = false;

    void Start()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is null! Make sure the UIManager is in the scene.");
            return;
        }

        UIManager.Instance.ClearAllTasks();
        
        // Add initial tasks
        taskList.Add(UIManager.Instance.AddTask("Press Q to switch modes"));
        taskList.Add(UIManager.Instance.AddTask("Press 1-3 to switch weapons"));
        taskList.Add(UIManager.Instance.AddTask("Press T to complete tasks"));
    }

    void Update()
    {
        if (tasksCompleted) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (taskList.Count > 0)
            {
                CompleteTask(taskList[0]);
            }
            else
            {
                Debug.Log("All tasks completed!");
                tasksCompleted = true;
            }
        }
    }

    void CompleteTask(GameObject task)
    {
        if (task == null || UIManager.Instance == null) return;

        taskList.Remove(task);
        UIManager.Instance.RemoveTask(task);
        Debug.Log($"Task completed! Remaining: {taskList.Count}");

        if (taskList.Count == 0)
        {
            Debug.Log("All tasks completed!");
            tasksCompleted = true;
        }
    }
}
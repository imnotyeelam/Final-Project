using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameTaskManager : MonoBehaviour
{
    private List<TaskItem> taskList = new List<TaskItem>();

    // Add a task with the given description
    public void AddGameTask(string description)
    {
        Debug.Log("Creating task: " + description);

        if (UIManager.Instance == null) return;

        TaskItem task = UIManager.Instance.AddTask(description);
        if (task != null)
        {
            taskList.Add(task);
        }
    }

    // Complete a specific task by its description
    public void CompleteTask(string description)
    {
        TaskItem taskToComplete = taskList.Find(task =>
            task != null && task.descriptionText != null && 
            task.descriptionText.text == description);

        if (taskToComplete != null)
        {
            taskToComplete.MarkCompleted();                     // UI feedback (e.g., tick or animation)
            UIManager.Instance.RemoveTask(taskToComplete);      // Remove from UI
            taskList.Remove(taskToComplete);                    // Remove from local list
        }
    }

    // Complete and remove all current tasks
    public void CompleteAllTasks()
    {
        foreach (var task in new List<TaskItem>(taskList)) // Copy list to avoid modification while iterating
        {
            if (task != null)
            {
                task.MarkCompleted();
                UIManager.Instance.RemoveTask(task);
            }
        }
        taskList.Clear();
    }
}

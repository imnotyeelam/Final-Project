using UnityEngine;
using System.Collections.Generic;

public class UITest : MonoBehaviour
{
    private List<GameObject> taskList = new List<GameObject>();

    void Start()
    {
        UIManager.Instance.ClearAllTasks();

        taskList.Add(UIManager.Instance.AddTask("q"));
        taskList.Add(UIManager.Instance.AddTask("1"));
        taskList.Add(UIManager.Instance.AddTask("2"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (taskList.Count > 0)
            {
                GameObject finishedTask = taskList[0];
                UIManager.Instance.RemoveTask(finishedTask);
                taskList.RemoveAt(0);
                Debug.Log("task done! left:" + taskList.Count);
            }
            else
            {
                Debug.Log("all task done!");
            }
        }
    }
}

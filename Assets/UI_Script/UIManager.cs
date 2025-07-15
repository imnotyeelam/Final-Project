using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("PlayerStatus UI")]
    public Slider healthBar;
    public Text healthText;
    public Text piecesText;

    [Header("TaskPanel UI")]
    public Transform taskContent;    // ScrollView Content
    public GameObject taskPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateHealth(float current, float max)
    {
        if (healthBar) healthBar.value = current / max;
        if (healthText) healthText.text = $"{current}/{max}";
    }

    public void UpdatePieces(int collected, int total)
    {
        if (piecesText) piecesText.text = $"Pieces: {collected}/{total}";
    }

    public GameObject AddTask(string taskName)
    {
        if (!taskPrefab) Debug.LogError("taskPrefab no variable?");
        if (!taskContent) Debug.LogError("taskContent no variable!");

        GameObject newTask = Instantiate(taskPrefab, taskContent);

        Text txt = newTask.GetComponentInChildren<Text>();
        if (!txt) Debug.LogError("taskPrefab no text!");

        if (txt) txt.text = taskName;

        return newTask;
    }



    public void RemoveTask(GameObject taskItem)
    {
        Destroy(taskItem);
    }

    public void ClearAllTasks()
    {
        foreach (Transform child in taskContent)
        {
            Destroy(child.gameObject);
        }
    }

}

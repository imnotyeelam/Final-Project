using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player Status UI")]
    public Slider healthBar;
    public Text healthText;
    public Slider energyBar;
    public Text piecesText;

    [Header("TaskPanel UI")]
    public Transform taskContent;
    public GameObject taskPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateHealth(float current, float max)
    {
        float percent = current / max;

        if (healthBar)
        {
            healthBar.value = percent;

            Color highColor = Color.green;
            Color midColor = Color.yellow;
            Color lowColor = Color.red;

            Image fillImage = healthBar.fillRect.GetComponent<Image>();

            if (percent > 0.5f)
            {
                float t = (percent - 0.5f) / 0.5f;
                fillImage.color = Color.Lerp(midColor, highColor, t);
            }
            else
            {
                float t = percent / 0.5f;
                fillImage.color = Color.Lerp(lowColor, midColor, t);
            }
        }

        if (healthText) healthText.text = $"{current}/{max}";
    }


    public void UpdateEnergy(float current, float max)
    {
        float percent = current / max;

        if (energyBar)
        {
            energyBar.value = percent;

            Color highColor = Color.cyan;
            Color midColor = Color.yellow;
            Color lowColor = Color.red;

            Image fillImage = energyBar.fillRect.GetComponent<Image>();

            if (percent > 0.5f)
            {
                float t = (percent - 0.5f) / 0.5f;
                fillImage.color = Color.Lerp(midColor, highColor, t);
            }
            else
            {
                float t = percent / 0.5f;
                fillImage.color = Color.Lerp(lowColor, midColor, t);
            }
        }
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

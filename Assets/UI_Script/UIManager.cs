using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Vitals")]
    public Slider healthSlider;
    public Slider energySlider;
    public Text piecesText;

    [Header("Prop Icons")]
    public GameObject ammoPanel;
    public GameObject heartPanel;
    public GameObject energyPanel;

    [Header("Task Panel")]
    public Transform taskPanel;
    public GameObject taskPrefab;

    private List<GameObject> currentTasks = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void UpdateHealth(float current, float max)
    {
        healthSlider.value = Mathf.Clamp01(current / max);
    }

    public void UpdateEnergy(float current, float max)
    {
        energySlider.value = Mathf.Clamp01(current / max);
    }

    public void UpdatePieces(int collected, int total)
    {
        piecesText.text = $"Pieces: {collected}/{total}";
    }

    public void AddAmmoIcon() => ammoPanel.SetActive(true);
    public void AddHeartIcon() => heartPanel.SetActive(true);
    public void AddEnergyIcon() => energyPanel.SetActive(true);

    public void ClearAllTasks()
    {
        foreach (var task in currentTasks)
            Destroy(task);

        currentTasks.Clear();
    }

    public GameObject AddTask(string description)
    {
        GameObject newTask = Instantiate(taskPrefab, taskPanel);
        Text taskText = newTask.GetComponentInChildren<Text>();
        if (taskText != null)
            taskText.text = description;

        currentTasks.Add(newTask);
        return newTask;
    }

    public void RemoveTask(GameObject task)
    {
        if (currentTasks.Contains(task))
        {
            currentTasks.Remove(task);
            Destroy(task);
        }
    }

    [System.Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ammoPanel.SetActive(false);
            WeaponManager.Instance?.RefillAmmo();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            heartPanel.SetActive(false);
            FindObjectOfType<PlayerVitalsManager>()?.AddHP(20f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            energyPanel.SetActive(false);
            FindObjectOfType<PlayerVitalsManager>()?.AddEnergy(20f);
        }
    }
}
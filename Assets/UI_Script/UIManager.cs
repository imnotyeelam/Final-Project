using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player Status UI")]
    public Slider healthBar;
    public Text healthText;
    public Slider energyBar;
    public Text piecesText;

    [Header("Task UI Components")]
    public Transform taskListParent;      // Where task items will appear
    public GameObject taskItemPrefab;     // Prefab with TaskItem script

    [Header("Toggle Panel")]
    public GameObject taskPanel;
    public KeyCode toggleKey = KeyCode.T;

    [Header("Props UI")]
    public Text ammoPropText;
    public Text hpPropText;
    public Text energyPropText;

    [Header("Weapon UI")]
    public GameObject outOfAmmoWarning;

    [Header("Ammo UI")]
    public GameObject ammoPanel;
    public Text ammoText;

    private int ammoProps = 0;
    private int hpProps = 0;
    private int energyProps = 0;

    // This will just store all task UI items
    public static List<TaskItem> taskList = new List<TaskItem>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Debug.Log("UIManager initialized");
    }

    void Update()
    {
        // Toggle Task Panel visibility
        if (Input.GetKeyDown(toggleKey) && taskPanel != null)
        {
            taskPanel.SetActive(!taskPanel.activeSelf);
        }
    }

    // ---------------- Player HP / Energy ----------------
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

        if (healthText)
            healthText.text = $"{Mathf.Ceil(current)}/{Mathf.Ceil(max)}";
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
        if (piecesText)
            piecesText.text = $"Pieces: {collected}/{total}";
    }

    // ---------------- Tasks ----------------
    public TaskItem AddTask(string description)
    {
        Debug.Log("Adding task: " + description);

        if (!taskItemPrefab || !taskListParent)
        {
            Debug.LogError("Task prefab or parent not assigned!");
            return null;
        }

        // Create new Task UI
        GameObject newTaskGO = Instantiate(taskItemPrefab, taskListParent);
        TaskItem taskItem = newTaskGO.GetComponent<TaskItem>();

        if (taskItem == null)
        {
            Debug.LogError("Task prefab missing TaskItem script.");
            return null;
        }

        taskItem.Setup(description);

        // Add to list for reference
        taskList.Add(taskItem);
        return taskItem;
    }

    public void ClearAllTasks()
    {
        if (taskListParent == null) return;

        foreach (Transform child in taskListParent)
        {
            Destroy(child.gameObject);
        }
        taskList.Clear();
    }

    // ---------------- Props ----------------
    public void AddProp(string type)
    {
        switch (type)
        {
            case "Ammo": ammoProps++; break;
            case "HP": hpProps++; break;
            case "Energy": energyProps++; break;
        }
        UpdatePropsUI();
    }

    public bool UseProp(string type)
    {
        bool success = false;

        switch (type)
        {
            case "Ammo":
                if (ammoProps > 0) { ammoProps--; success = true; }
                break;
            case "HP":
                if (hpProps > 0) { hpProps--; success = true; }
                break;
            case "Energy":
                if (energyProps > 0) { energyProps--; success = true; }
                break;
        }

        if (success) UpdatePropsUI();
        return success;
    }

    private void UpdatePropsUI()
    {
        if (ammoPropText) ammoPropText.text = $"{ammoProps}";
        if (hpPropText) hpPropText.text = $"{hpProps}";
        if (energyPropText) energyPropText.text = $"{energyProps}";
    }

    public void ClearAllProps()
    {
        ammoProps = 0;
        hpProps = 0;
        energyProps = 0;
        UpdatePropsUI();
    }

    // ---------------- Weapon & Ammo ----------------
    public void ShowOutOfAmmo(bool show)
    {
        if (outOfAmmoWarning)
            outOfAmmoWarning.SetActive(show);
    }

    public void UpdateAmmoUI(int current, int max)
    {
        if (ammoText)
            ammoText.text = $"{current}/{max}";

        if (current > 0 && outOfAmmoWarning)
            outOfAmmoWarning.SetActive(false);
    }
}

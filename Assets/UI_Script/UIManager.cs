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

    [Header("Props UI")]
    public Text ammoPropText;
    public Text hpPropText;
    public Text energyPropText;

    [Header("Weapon UI")]
    public GameObject outOfAmmoWarning;  // Drag your No Ammo panel here

    [Header("Ammo UI")]
    public GameObject ammoPanel;   // Only visible when gun is equipped
    public Text ammoText;          // Displays ammo count


    private int ammoProps = 0;
    private int hpProps = 0;
    private int energyProps = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

    // ---------------- Tasks ----------------
    public GameObject AddTask(string taskName)
    {
        if (!taskPrefab) Debug.LogError("taskPrefab not assigned!");
        if (!taskContent) Debug.LogError("taskContent not assigned!");

        GameObject newTask = Instantiate(taskPrefab, taskContent);

        Text txt = newTask.GetComponentInChildren<Text>();
        if (!txt) Debug.LogError("taskPrefab has no text component!");

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
                if (ammoProps > 0)
                {
                    ammoProps--;
                    success = true;
                    WeaponManager.Instance.AddAmmo(0);
                }
                break;

            case "HP":
                if (hpProps > 0) { hpProps--; success = true; }
                if (hpProps > 0)
                {
                    hpProps--;
                    success = true;
                }
                break;

            case "Energy":
                if (energyProps > 0)
                {
                    energyProps--;
                    success = true;
                }
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

    // ---------------- Weapon & Ammo ----------------
    public void ShowOutOfAmmo(bool show)
    {
        if (outOfAmmoWarning)
            outOfAmmoWarning.SetActive(show);
    }

    public void UpdateAmmoUI(int current, int max)
    {
        if (ammoText) ammoText.text = $"{current}";

        if (current > 0 && outOfAmmoWarning)
            outOfAmmoWarning.SetActive(false);
    }
}

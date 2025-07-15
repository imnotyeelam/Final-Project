using UnityEngine;

public class UIManagerTest : MonoBehaviour
{
    private float currentHP = 100f;
    private float maxHP = 100f;

    private float currentEnergy = 50f;
    private float maxEnergy = 100f;

    private int collectedPieces = 0;
    private int totalPieces = 5;

    void Start()
    {
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHP = Mathf.Max(0, currentHP - 10);
            UIManager.Instance.UpdateHealth(currentHP, maxHP);
            Debug.Log($"{currentHP}/{maxHP}");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            currentHP = Mathf.Min(maxHP, currentHP + 10);
            UIManager.Instance.UpdateHealth(currentHP, maxHP);
            Debug.Log($"{currentHP}/{maxHP}");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentEnergy = Mathf.Max(0, currentEnergy - 5);
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
            Debug.Log($"{currentEnergy}/{maxEnergy}");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + 5);
            UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
            Debug.Log($"{currentEnergy}/{maxEnergy}");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (collectedPieces < totalPieces)
            {
                collectedPieces++;
                UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
                Debug.Log($"Pieces {collectedPieces}/{totalPieces}");
            }
            else
            {
                Debug.Log("shou ji wan la!");
            }
        }
    }
}

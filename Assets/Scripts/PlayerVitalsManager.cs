using UnityEngine;

public class PlayerVitalsManager : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxEnergy = 100f;
    public float currentHP;
    public float currentEnergy;

    public int totalPieces = 3;
    public int collectedPieces = 0;

    void Start()
    {
        currentHP = maxHP;
        currentEnergy = maxEnergy;

        // Init UI
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
        UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
    }

    public void AddHP(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
    }

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        UIManager.Instance.UpdateHealth(currentHP, maxHP);
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
        UIManager.Instance.UpdateEnergy(currentEnergy, maxEnergy);
    }

    public void CollectPiece()
    {
        if (collectedPieces < totalPieces)
        {
            collectedPieces++;
            UIManager.Instance.UpdatePieces(collectedPieces, totalPieces);
        }
    }
}

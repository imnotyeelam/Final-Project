using UnityEngine;

public class PropCollector : MonoBehaviour
{
    public PlayerStatsManager statsManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            statsManager.Heal(20);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ammo"))
        {
            statsManager.AddAmmo(10);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Energy"))
        {
            statsManager.AddEnergy(15);
            Destroy(other.gameObject);
        }
    }
}
using UnityEngine;

public class PropCollector : MonoBehaviour
{
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HPProp"))
        {
            UIManager.Instance.AddProp("HP");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnergyProp"))
        {
            UIManager.Instance.AddProp("Energy");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("AmmoProp"))
        {
            UIManager.Instance.AddProp("Ammo");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Piece"))
        {
            FindObjectOfType<PlayerVitalsManager>().CollectPiece();
            Destroy(other.gameObject);
        }
    }
}

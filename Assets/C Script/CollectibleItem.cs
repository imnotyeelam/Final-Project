using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private bool isInRange = false;
    private Transform player;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();
            if (inv != null)
            {
                inv.Collect(gameObject.name); // Or use a custom item name
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}

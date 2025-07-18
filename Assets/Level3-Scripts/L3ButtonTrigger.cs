using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject stairs;  // ����¥�ݶ���
    private bool playerInRange = false;

    void Start()
    {
        if (stairs != null)
            stairs.SetActive(false);  // ��ʼ����
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateStairs();
        }
    }

    void ActivateStairs()
    {
        if (stairs != null)
        {
            stairs.SetActive(true);
            Debug.Log("¥���ѳ��֣�");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

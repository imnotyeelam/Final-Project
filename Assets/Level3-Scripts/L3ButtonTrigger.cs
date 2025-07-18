using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject stairs;  // 拖入楼梯对象
    private bool playerInRange = false;

    void Start()
    {
        if (stairs != null)
            stairs.SetActive(false);  // 初始隐藏
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
            Debug.Log("楼梯已出现！");
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

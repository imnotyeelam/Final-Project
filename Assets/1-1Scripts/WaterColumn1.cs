using UnityEngine;

public class WaterColumn1 : MonoBehaviour
{
    public float swimUpSpeed = 2f;
    private bool isInWaterColumn = false;
    private CharacterController characterController;
    private PlayerController1 playerMovement; // 你的人物移动脚本

    void Start()
    {
        characterController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        playerMovement = characterController.GetComponent<PlayerController1>(); // 替换成你的具体类名
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInWaterColumn = true;
            playerMovement.enabled = false; // 可选：禁用普通移动逻辑
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInWaterColumn = false;
            playerMovement.enabled = true;
        }
    }

    void Update()
    {
        if (isInWaterColumn && Input.GetKey(KeyCode.Space))
        {
            characterController.Move(Vector3.up * swimUpSpeed * Time.deltaTime);
        }
    }
}

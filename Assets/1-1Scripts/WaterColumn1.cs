using UnityEngine;

public class WaterColumn1 : MonoBehaviour
{

    public float swimUpSpeed = 2f;
    private bool isInWaterColumn = false;
    private CharacterController characterController;
    private PlayerController1 playerMovement; // 你的人物移动脚本

    void Start()
    {

        characterController = GameObject.FindWithTag("Player").GetComponentInParent<CharacterController>();
        playerMovement = characterController.GetComponent<PlayerController1>(); // 替换成你的具体类名
        if (characterController == null || playerMovement == null)
        {
            Debug.LogError("进入水柱的对象缺少必要组件");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("可以游泳");
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
        if (isInWaterColumn && Input.GetKey(KeyCode.W))//按下w可以向上游动
        {
            characterController.Move(Vector3.up * swimUpSpeed * Time.deltaTime);
        }
    }
}
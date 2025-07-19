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
        if (isInWaterColumn)
        {
            Vector3 move = Vector3.zero;

            // 向上游动
            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.up * swimUpSpeed;
            }
            else
            {
                // 模拟水下的重力
                move += Vector3.down * swimUpSpeed * 0.5f; // 你可以调整这个0.5让下落更慢或更快
            }

            // 左右移动
            if (Input.GetKey(KeyCode.A))
            {
                move += Vector3.left * swimUpSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector3.right * swimUpSpeed;
            }

            // 应用移动
            characterController.Move(move * Time.deltaTime);
        }
    }

}

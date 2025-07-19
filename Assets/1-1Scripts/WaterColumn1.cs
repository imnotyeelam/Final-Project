using UnityEngine;



public class WaterColumn1 : MonoBehaviour
{

    public float swimUpSpeed = 2f;
    private bool isInWaterColumn = false;
    private CharacterController characterController;
    private PlayerController1 playerMovement; // ��������ƶ��ű�

    void Start()
    {

        characterController = GameObject.FindWithTag("Player").GetComponentInParent<CharacterController>();
        playerMovement = characterController.GetComponent<PlayerController1>(); // �滻����ľ�������
        if (characterController == null || playerMovement == null)
        {
            Debug.LogError("����ˮ���Ķ���ȱ�ٱ�Ҫ���");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("������Ӿ");
            isInWaterColumn = true;
            playerMovement.enabled = false; // ��ѡ��������ͨ�ƶ��߼�
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

            // �����ζ�
            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.up * swimUpSpeed;
            }
            else
            {
                // ģ��ˮ�µ�����
                move += Vector3.down * swimUpSpeed * 0.5f; // ����Ե������0.5��������������
            }

            // �����ƶ�
            if (Input.GetKey(KeyCode.A))
            {
                move += Vector3.left * swimUpSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector3.right * swimUpSpeed;
            }

            // Ӧ���ƶ�
            characterController.Move(move * Time.deltaTime);
        }
    }

}

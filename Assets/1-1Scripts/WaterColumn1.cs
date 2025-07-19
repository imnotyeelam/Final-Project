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
        if (isInWaterColumn && Input.GetKey(KeyCode.W))//����w���������ζ�
        {
            characterController.Move(Vector3.up * swimUpSpeed * Time.deltaTime);
        }
    }
}
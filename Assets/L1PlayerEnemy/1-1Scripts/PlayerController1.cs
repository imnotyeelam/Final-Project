using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public static PlayerController1 instance;

    public float moveSpeed = 8f;
    public float gravityModifier = 2f;
    public float jumpPower = 8f;
    public float runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;
    public Transform camTrans;
    public float mouseSensitivity = 3f;

    private int Jumpcount = 2;
    private int maxJump = 2;

    private bool nearPlane = false;
    private L3PlaneController planeController;
    private bool isFlying = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // ������ڷ��У�������������
        if (isFlying) return;

        // ��ҵ����ƶ�
        float yStore = moveInput.y;
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = (horMove + vertMove).normalized;
        moveInput *= Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (charCon.isGrounded)
        {
            Jumpcount = maxJump;
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Jumpcount > 0)
        {
            moveInput.y = jumpPower;
            Jumpcount--;
        }

        charCon.Move(moveInput * Time.deltaTime);

        // �������ת
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + mouseInput.x, 0);
        camTrans.localRotation = Quaternion.Euler(camTrans.localRotation.eulerAngles.x - mouseInput.y, 0, 0);

        // �Ϸɻ��߼�
        if (nearPlane && Input.GetKeyDown(KeyCode.E))
        {
            EnterPlane();
        }
    }

    void EnterPlane()
    {
        Debug.Log("�л����ɻ�ģʽ");
        isFlying = true;
        charCon.enabled = false; // ���ý�ɫ�������
        planeController.ActivatePlane(transform); // �ɻ��ӹ����λ��
    }

    public void ExitPlane()
    {
        Debug.Log("�ص�����ģʽ");
        isFlying = false;
        charCon.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            nearPlane = true;
            planeController = other.GetComponent<L3PlaneController>();
            Debug.Log("�����ɻ������԰�E����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            nearPlane = false;
            planeController = null;
            Debug.Log("�뿪�ɻ�");
        }
    }
}

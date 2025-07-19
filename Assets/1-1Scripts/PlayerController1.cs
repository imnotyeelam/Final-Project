using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController1 : MonoBehaviour
{
    public static PlayerController1 instance;
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;

    private int Jumpcount = 2;
    private int maxJump = 2;

    public GameObject bullet;
    public Transform firePoint;//spawn point of the bullet

    //public Gun activeGun;
    //public List<Gun> allGuns = new List<Gun>();//比起array，list更灵活，根据存储的variable留出对应的memory
    public int currentGun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标在屏幕中心
        Cursor.visible = false; // 隐藏鼠标
    }

    // Update is called once per frame
    void Update()
    {
        if (!charCon.enabled) return; // ✅ 加这一句避免报错

        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horMove + vertMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (charCon.isGrounded)
        {
            Jumpcount = maxJump;
            moveInput.y = 0;
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Jumpcount > 0)
        {
            moveInput.y = jumpPower;
            Jumpcount--;
        }

        charCon.Move(moveInput * Time.deltaTime);

        // Mouse rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles.x - mouseInput.y, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);

        // Shooting
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 500f))
            {
                firePoint.LookAt(hit.point);
            }
            else
            {
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
            }

            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

}

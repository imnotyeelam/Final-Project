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
        float yStore = moveInput.y;//Store the first position moveInput y of the player

        //to prevent falling error movement when the player satrt from higher ground

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");//forward means z axis
        Vector3 horMove = transform.right * Input.GetAxis("Horizontal");//right means x axis

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
        moveInput.y = yStore;//continue the moveInput y;


        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        if (charCon.isGrounded)//To detect the ground
        {
            Jumpcount = maxJump;
            moveInput.y = 0;//normalize our y input
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;//Apply gravity

        }
        if (Input.GetKeyDown(KeyCode.Space) && Jumpcount > 0) //if the player press space bar
        {
            moveInput.y = jumpPower;//will make jumping movement
            Jumpcount--;
        }

        charCon.Move(moveInput * Time.deltaTime);

        //rotation controll
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        //+ mouse input x will influence the y rotation

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles.x - mouseInput.y, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
        //camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles+ new Vector3(-moveInput.y, 0f, 0f));这个目前没法用

        //Debug.Log("Speed = " + moveInput.magnitude);

        //Handle the shooting
        
        if(Input.GetMouseButtonDown(0) )//left click mouse button
        {
            
            RaycastHit hit;//invisible stick
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 500f))
            {
                firePoint.LookAt(hit.point);
            }
            else
            {
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
            }
            
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            //spawn the bullet on the fire pointposition and rotation
            //FireShot();
        }
        
    }
}

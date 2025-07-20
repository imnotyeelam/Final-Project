using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public static PlayerController1 instance;
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform cameraPoint; // 改为引用Main Camera上的Camera Point
    private Camera mainCamera; // 添加主摄像机引用

    public float mouseSensitivity;

    private int Jumpcount = 2;
    private int maxJump = 2;

    private Animator anim;

    public GameObject bullet;
    public Transform firePoint; //spawn point of the bullet

    //Plane controller
    private bool isOnPlatform = false; // 是否在平台上
    private Transform currentPlatform; // 当前绑定的平台

    //public Gun activeGun;
    //public List<Gun> allGuns = new List<Gun>();//比起array，list更灵活，根据存储的variable留出对应的memory
    public int currentGun;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main; // 获取主摄像机
    }

    void Start()
    {
        //activeGun = allGuns[currentGun];
        //activeGun.gameObject.SetActive(true);

        //UIController.instance.AmmoText.text = "Ammo: " + activeGun.currentAmmo;
        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 摄像机旋转逻辑（即使在平台上也能看周围）
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        // 使用Camera Point控制摄像机旋转
        if (cameraPoint != null)
        {
            cameraPoint.rotation = Quaternion.Euler(cameraPoint.rotation.eulerAngles.x - mouseInput.y,
                                                  cameraPoint.rotation.eulerAngles.y,
                                                  cameraPoint.rotation.eulerAngles.z);

            // 同步主摄像机位置和旋转到Camera Point
            mainCamera.transform.position = cameraPoint.position;
            mainCamera.transform.rotation = cameraPoint.rotation;
        }

        // 如果在平台上，只能按 E 下飞机
        if (isOnPlatform)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitPlatform();
            }
            return; // 禁用WASD
        }

        // 玩家普通移动逻辑
        float yStore = moveInput.y; //Store the first position moveInput y of the player

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
        moveInput.y = yStore; //continue the moveInput y;

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

        // 射击逻辑
        if (Input.GetMouseButtonDown(0))//left click mouse button
        {
            RaycastHit hit;//invisible stick
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 500f))
            {
                firePoint.LookAt(hit.point);
            }
            else
            {
                firePoint.LookAt(mainCamera.transform.position + (mainCamera.transform.forward * 30f));
            }

            Instantiate(bullet, firePoint.position, firePoint.rotation);
            //spawn the bullet on the fire pointposition and rotation
            //FireShot();
        }
    }

    // 上平台
    public void EnterPlatform(Transform platform)
    {
        isOnPlatform = true;
        currentPlatform = platform;
        transform.SetParent(platform); // 绑定到平台
        charCon.enabled = false; // 停止 CharacterController
        Debug.Log("玩家已上平台");
    }

    // 下平台
    public void ExitPlatform()
    {
        isOnPlatform = false;
        transform.SetParent(null); // 解除绑定
        charCon.enabled = true; // 恢复 CharacterController

        Vector3 exitOffset = currentPlatform.right * 2f; // 右移2m
        transform.position = currentPlatform.position + exitOffset;

        L3MovingPlane plane = currentPlatform.GetComponent<L3MovingPlane>();
        if (plane != null)
        {
            plane.TemporarilyDisableAttach(3f); // 禁用3秒
        }

        Debug.Log("玩家已下平台");
    }
}
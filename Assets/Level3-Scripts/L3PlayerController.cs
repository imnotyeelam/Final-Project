using UnityEngine;

public class L3PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController charCon;

    private Vector3 moveInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        charCon.Move(moveInput);
    }
}

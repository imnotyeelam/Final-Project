using UnityEngine;

public class Target1 : MonoBehaviour
{
    public int targetID; // 1 = 门, 2 = 平台, 3 = 门变色
    public Animator anim2;

    void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("打到靶子");
            anim2.SetBool("Hit", true); // 靶子动
        }
    }
}

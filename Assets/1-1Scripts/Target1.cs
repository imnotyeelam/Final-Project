using UnityEngine;

public class Target1 : MonoBehaviour
{
    public int targetID; // 1 = ��, 2 = ƽ̨, 3 = �ű�ɫ
    public Animator anim2;

    void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("�򵽰���");
            anim2.SetBool("Hit", true); // ���Ӷ�
        }
    }
}

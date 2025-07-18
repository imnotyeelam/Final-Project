using UnityEngine;

public class Target1 : MonoBehaviour
{
    public int targetID; // 1 = ��, 2 = ƽ̨, 3 = �ű�ɫ
    public TargetManager1 manager;
    public Animator anim2;
    private bool hasBeenHit = false; // ��ֹ�ظ�����
    void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            anim2.SetBool("Hit", true); // ÿ�ζ������ܻ�����
            Debug.Log("�򵽰���");

            if (!hasBeenHit) // ֻ�е�һ�β�֪ͨ manager
            {
                manager.OnTargetHit(targetID);
                hasBeenHit = true; // ���Ϊ�����У��������ٵ��� manager
            }
        }
    }
}

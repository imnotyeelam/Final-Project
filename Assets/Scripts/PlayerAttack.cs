using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //目前里面没有引用的但是有一个attackMultiplier，保险起见保留，保险起见挂在player上
    public Animator handAnimator;
    private bool isShooting = false;
    public float attackMultiplier = 1f; // <-- added multiplier field

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isShooting = !isShooting;
            if (handAnimator != null)
                handAnimator.SetBool("IsHooking", isShooting);
        }

        // Example: actual shooting with damage = baseDamage * attackMultiplier
    }
}

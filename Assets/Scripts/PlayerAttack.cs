using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator handAnimator;
    private bool isShooting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isShooting = !isShooting; // Toggle shooting mode

            if (handAnimator != null)
            {
                handAnimator.SetBool("IsHooking", isShooting);
            }
        }
    }
}

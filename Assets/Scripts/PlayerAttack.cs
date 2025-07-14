using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator handAnimator;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Left mouse button
        {
            if (handAnimator != null)
            {
                handAnimator.SetTrigger("Attack");
            }
        }
    }
}

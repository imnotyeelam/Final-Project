using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
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

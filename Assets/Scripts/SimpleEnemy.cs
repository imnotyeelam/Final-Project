using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int health = 2; // Default health (can customize per enemy in Inspector)

    // Call this method from PlayerBullet when hit
    public void TakeDamage(int damage)
    {
        health -= damage;

        // Optional: play hit animation, flash color, etc.
        if (health <= 0)
        {
            // Leave destruction to bullet script or external handler
            Debug.Log($"{gameObject.name} defeated!");
        }
    }
}

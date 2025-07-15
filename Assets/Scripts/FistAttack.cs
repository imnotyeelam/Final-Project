using UnityEngine;

public class FistAttack : MonoBehaviour
{
    public float punchRange = 2f;
    public int punchDamage = 1;
    public float punchCooldown = 0.5f;
    public Transform leftFistPoint;
    public Transform rightFistPoint;
    public GameObject punchEffect;

    private float lastPunchTime = 0f;
    private bool useLeftHand = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastPunchTime >= punchCooldown)
        {
            lastPunchTime = Time.time;

            if (useLeftHand)
                Punch(leftFistPoint);
            else
                Punch(rightFistPoint);

            useLeftHand = !useLeftHand;
        }
    }

    void Punch(Transform fistPoint)
    {
        Debug.Log($"Punching with {(useLeftHand ? "Left" : "Right")} hand");

        // Optional: show punch effect
        if (punchEffect != null)
        {
            Instantiate(punchEffect, fistPoint.position, fistPoint.rotation);
        }

        // Detect all nearby colliders within punch range
        Collider[] hits = Physics.OverlapSphere(fistPoint.position, punchRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                SimpleEnemy enemy = hit.GetComponent<SimpleEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(punchDamage);
                    Debug.Log("Enemy hit by punch!");

                    if (enemy.health <= 0)
                    {
                        Destroy(hit.gameObject);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (leftFistPoint != null)
            Gizmos.DrawWireSphere(leftFistPoint.position, punchRange);
        if (rightFistPoint != null)
            Gizmos.DrawWireSphere(rightFistPoint.position, punchRange);
    }
}

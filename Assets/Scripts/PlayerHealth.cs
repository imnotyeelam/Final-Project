using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject playerModel;
    public GameObject cameraHolder;
    public float deathFallSpeed = 2f;

    private MonoBehaviour gunScript;
    private MonoBehaviour hookScript;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Automatically find disabled components too
        gunScript = GetComponentInChildren<GunShooter>(true);
        hookScript = GetComponentInChildren<GrapplingHook>(true);
    }

    [System.Obsolete]
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [System.Obsolete]
    void Die()
    {
        isDead = true;

        // Disable player movement and abilities
        GetComponent<SimpleFPSMovement>().enabled = false;
        GetComponent<CharacterController>().enabled = false;

        if (gunScript) gunScript.enabled = false;
        if (hookScript) hookScript.enabled = false;

        if (playerModel) playerModel.SetActive(false);

        if (cameraHolder)
            StartCoroutine(FallAndTiltCamera());

        // Switch to dead state: No hands
        FindObjectOfType<HandSwitcher>()?.SwitchToDeadState();
    }

    System.Collections.IEnumerator FallAndTiltCamera()
    {
        float duration = 1f;
        float timer = 0f;

        Vector3 startPos = cameraHolder.transform.position;
        Vector3 endPos = startPos + Vector3.down * 1.5f;

        Quaternion startRot = cameraHolder.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, 30f);

        while (timer < duration)
        {
            float t = timer / duration;

            cameraHolder.transform.position = Vector3.Lerp(startPos, endPos, t);
            cameraHolder.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            timer += Time.deltaTime * deathFallSpeed;
            yield return null;
        }
    }
}

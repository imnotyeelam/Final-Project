using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Death Visuals")]
    public GameObject playerModel;         // Player's visible body
    public GameObject cameraHolder;        // Usually the CameraHolder object
    public float deathFallSpeed = 2f;

    [Header("Fade Effect")]
    public CanvasGroup fadeCanvasGroup;    // A UI panel with CanvasGroup for fade
    public float fadeDuration = 2f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Optional: Ensure fade screen starts invisible
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;
    }

    [System.Obsolete]
    void Update()
    {
        // Optional: Press K to test death manually
        if (Input.GetKeyDown(KeyCode.K) && !isDead)
        {
            TakeDamage(maxHealth);
        }
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

        // Disable movement and control scripts
        var move = GetComponent<SimpleFPSMovement>() ?? GetComponentInParent<SimpleFPSMovement>();
        if (move != null) move.enabled = false;

        var controller = GetComponent<CharacterController>() ?? GetComponentInParent<CharacterController>();
        if (controller != null) controller.enabled = false;

        var gun = GetComponentInChildren<GunShooter>();
        if (gun != null) gun.enabled = false;

        var hook = GetComponentInChildren<GrapplingHook>();
        if (hook != null) hook.enabled = false;

        var camController = GetComponentInChildren<FPSCameraController>();
        if (camController != null) camController.enabled = false;

        // Disable player model
        if (playerModel) playerModel.SetActive(false);

        // Switch to dead camera hands
        HandSwitcher handSwitcher = FindObjectOfType<HandSwitcher>();
        if (handSwitcher != null)
        {
            handSwitcher.SwitchToDeadState();
        }

        // Tilt the camera down
        if (cameraHolder != null)
        {
            StartCoroutine(FallDown());
        }

        // Start screen fade out
        if (fadeCanvasGroup != null)
        {
            StartCoroutine(FadeOutAndRestart());
        }
    }

    IEnumerator FallDown()
    {
        float timer = 0f;
        Vector3 startPos = cameraHolder.transform.position;
        Vector3 endPos = startPos + Vector3.down * 1.5f;

        while (timer < 1f)
        {
            cameraHolder.transform.position = Vector3.Lerp(startPos, endPos, timer);
            timer += Time.deltaTime * deathFallSpeed;
            yield return null;
        }
    }

    IEnumerator FadeOutAndRestart()
    {
        yield return new WaitForSeconds(1f); // Small delay before fade

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Optional: wait before restart

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

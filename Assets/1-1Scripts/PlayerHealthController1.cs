using UnityEngine;
using System.Collections;

public class PlayerHealthController1 : MonoBehaviour, Boss1IDamageable
{
    public static PlayerHealthController1 instance;

    public int maxHealth = 10;
    public int currentHealth;

    public float invLength = 1.0f; // 受击后的无敌时间
    private float inVCounter;

    public float respawnDelay = 1.0f; // 复活等待时间

    private bool isDead = false;
    private Renderer bodyRenderer; // 用来引用 Capsule 上的 Renderer

    private void Awake()
    {
        instance = this;

        // 找到子物体中的 Renderer（例如 Capsule）
        bodyRenderer = GetComponentInChildren<Renderer>();
    }


    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Update()
    {
        if (inVCounter > 0)
        {
            inVCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(RespawnAfterDelay());
        }
    }

    public void TakeDamage(int damage, bool attackPlayer)
    {
        if (isDead || !attackPlayer) return;

        if (inVCounter <= 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                StartCoroutine(RespawnAfterDelay());
            }

            inVCounter = invLength;
        }
    }

    public void HealPlayer(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        isDead = true;

        // 隐藏视觉
        if (bodyRenderer != null) bodyRenderer.enabled = false;

        // 禁用控制器
        if (PlayerController1.instance != null)
            PlayerController1.instance.GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // 重置血量
        currentHealth = maxHealth;

        // 找到当前存档的 checkpoint 名称
        string cpKey = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_cp";
        if (PlayerPrefs.HasKey(cpKey))
        {
            string checkpointName = PlayerPrefs.GetString(cpKey);
            if (L2CheckpointManager.checkpointDict.TryGetValue(checkpointName, out Transform checkpointTransform))
            {
                transform.position = checkpointTransform.position;
                Debug.Log("复活成功！");
            }
            else
            {
                Debug.LogWarning("找不到 checkpoint: " + checkpointName);
            }

        }

        // 显示视觉
        if (bodyRenderer != null) bodyRenderer.enabled = true;

        // 启用控制器
        if (PlayerController1.instance != null)
            PlayerController1.instance.GetComponent<CharacterController>().enabled = true;

        inVCounter = invLength;
        isDead = false;
    }



}

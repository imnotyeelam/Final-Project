using UnityEngine;
using System.Collections;

public class PlayerHealthController1 : MonoBehaviour, Boss1IDamageable
{
    public static PlayerHealthController1 instance;

    public int maxHealth = 10;
    public int currentHealth;

    public float invLength = 1.0f; // 受击后的无敌时间
    private float inVCounter;

    public float respawnDelay = 2.0f; // 复活等待时间

    private bool isDead = false;

    private void Awake()
    {
        instance = this;
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

        // 暂时隐藏玩家对象（可改成动画或特效）
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        // 重置血量
        currentHealth = maxHealth;

        // 传送回 Checkpoint
        L2CheckpointManager.RespawnPlayer(transform);

        // 重新启用玩家
        gameObject.SetActive(true);

        // 重置状态
        inVCounter = invLength;
        isDead = false;

        // 可选：播放复活动画、UI 提示、无敌闪烁等等
    }
}

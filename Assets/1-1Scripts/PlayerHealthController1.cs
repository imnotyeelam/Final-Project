using UnityEngine;
using System.Collections;

public class PlayerHealthController1 : MonoBehaviour, Boss1IDamageable
{
    public static PlayerHealthController1 instance;

    public int maxHealth = 10;
    public int currentHealth;

    public float invLength = 1.0f; // �ܻ�����޵�ʱ��
    private float inVCounter;

    public float respawnDelay = 2.0f; // ����ȴ�ʱ��

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

        // ��ʱ������Ҷ��󣨿ɸĳɶ�������Ч��
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        // ����Ѫ��
        currentHealth = maxHealth;

        // ���ͻ� Checkpoint
        L2CheckpointManager.RespawnPlayer(transform);

        // �����������
        gameObject.SetActive(true);

        // ����״̬
        inVCounter = invLength;
        isDead = false;

        // ��ѡ�����Ÿ������UI ��ʾ���޵���˸�ȵ�
    }
}

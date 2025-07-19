using UnityEngine;
using System.Collections;

public class PlayerHealthController1 : MonoBehaviour, Boss1IDamageable
{
    public static PlayerHealthController1 instance;

    public int maxHealth = 10;
    public int currentHealth;

    public float invLength = 1.0f; // �ܻ�����޵�ʱ��
    private float inVCounter;

    public float respawnDelay = 1.0f; // ����ȴ�ʱ��

    private bool isDead = false;
    private Renderer bodyRenderer; // �������� Capsule �ϵ� Renderer

    private void Awake()
    {
        instance = this;

        // �ҵ��������е� Renderer������ Capsule��
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

        // �����Ӿ�
        if (bodyRenderer != null) bodyRenderer.enabled = false;

        // ���ÿ�����
        if (PlayerController1.instance != null)
            PlayerController1.instance.GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // ����Ѫ��
        currentHealth = maxHealth;

        // �ҵ���ǰ�浵�� checkpoint ����
        string cpKey = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_cp";
        if (PlayerPrefs.HasKey(cpKey))
        {
            string checkpointName = PlayerPrefs.GetString(cpKey);
            if (L2CheckpointManager.checkpointDict.TryGetValue(checkpointName, out Transform checkpointTransform))
            {
                transform.position = checkpointTransform.position;
                Debug.Log("����ɹ���");
            }
            else
            {
                Debug.LogWarning("�Ҳ��� checkpoint: " + checkpointName);
            }

        }

        // ��ʾ�Ӿ�
        if (bodyRenderer != null) bodyRenderer.enabled = true;

        // ���ÿ�����
        if (PlayerController1.instance != null)
            PlayerController1.instance.GetComponent<CharacterController>().enabled = true;

        inVCounter = invLength;
        isDead = false;
    }



}

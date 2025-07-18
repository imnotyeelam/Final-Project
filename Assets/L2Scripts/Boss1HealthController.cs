using UnityEngine;
using System.Collections;

public class Boss1HealthController : MonoBehaviour
{
    public enum BossType { MainBoss, Clone }
    public BossType bossType = BossType.MainBoss;

    public int currentHealth = 20;
    public Vector3 spawnPosition;

    public GameObject portalPrefab;         // 添加这行
    public Transform portalSpawnPoint;      // 添加这行

    void Start()
    {
        spawnPosition = transform.position;

        if (bossType == BossType.Clone)
        {
            BossManager.instance.RegisterClone(this);
        }
    }

    public void DamageEnemy(int BulletDamage)
    {
        currentHealth -= BulletDamage;

        if (currentHealth <= 0)
        {
            if (bossType == BossType.MainBoss)
            {
                // 生成传送门
                if (portalPrefab != null)
                {
                    Vector3 spawnPos = portalSpawnPoint != null ? portalSpawnPoint.position : transform.position;
                    Instantiate(portalPrefab, spawnPos, Quaternion.identity);
                }

                BossManager.instance.MainBossDied();
                Destroy(gameObject);
            }
            else
            {
                if (!BossManager.instance.IsMainBossDead())
                {
                    BossManager.instance.StartReviveClone(this, 5f);
                }

                gameObject.SetActive(false);
            }

        }
    }

    public void ForceKill()
    {
        Destroy(gameObject);
    }

    private IEnumerator ReviveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentHealth = 20;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}

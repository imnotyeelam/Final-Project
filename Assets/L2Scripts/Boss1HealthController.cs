using UnityEngine;
using System.Collections; // 别忘了加这一行才能用 IEnumerator

public class Boss1HealthController : MonoBehaviour
{
    public enum BossType { MainBoss, Clone }
    public BossType bossType = BossType.MainBoss;

    public int currentHealth = 2;

    private Vector3 spawnPosition;

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
                BossManager.instance.MainBossDied();
                Destroy(gameObject);
            }
            else
            {
                if (!BossManager.instance.IsMainBossDead())
                    StartCoroutine(ReviveAfterSeconds(5f)); // 5秒后复活

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
        currentHealth = 2;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}

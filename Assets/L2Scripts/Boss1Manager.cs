using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    private bool mainBossDead = false;
    private List<Boss1HealthController> clones = new List<Boss1HealthController>();

    void Awake()
    {
        instance = this;
    }

    public void RegisterClone(Boss1HealthController clone)
    {
        if (!clones.Contains(clone))
        {
            clones.Add(clone);
        }
    }

    public void MainBossDied()
    {
        mainBossDead = true;

        // 强制销毁所有 Clone
        foreach (var clone in clones)
        {
            if (clone != null)
                clone.ForceKill();
        }
    }

    public bool IsMainBossDead()
    {
        return mainBossDead;
    }

    /// <summary>
    /// 在 Manager 中统一处理 Clone 的复活逻辑
    /// </summary>
    public void StartReviveClone(Boss1HealthController clone, float delay)
    {
        StartCoroutine(ReviveCloneAfterSeconds(clone, delay));
    }

    private IEnumerator ReviveCloneAfterSeconds(Boss1HealthController clone, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // 如果主 Boss 没死，才复活 Clone
        if (!mainBossDead && clone != null)
        {
            clone.currentHealth = 20;
            clone.transform.position = clone.spawnPosition; // 确保 spawnPosition 是 public 或有 getter
            clone.gameObject.SetActive(true);
        }
    }

    // （可选）清空 clone 列表中已经被销毁的对象
    public void CleanUpClones()
    {
        clones.RemoveAll(clone => clone == null);
    }
}

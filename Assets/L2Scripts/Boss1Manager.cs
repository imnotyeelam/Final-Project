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

        // ǿ���������� Clone
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
    /// �� Manager ��ͳһ���� Clone �ĸ����߼�
    /// </summary>
    public void StartReviveClone(Boss1HealthController clone, float delay)
    {
        StartCoroutine(ReviveCloneAfterSeconds(clone, delay));
    }

    private IEnumerator ReviveCloneAfterSeconds(Boss1HealthController clone, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // ����� Boss û�����Ÿ��� Clone
        if (!mainBossDead && clone != null)
        {
            clone.currentHealth = 20;
            clone.transform.position = clone.spawnPosition; // ȷ�� spawnPosition �� public ���� getter
            clone.gameObject.SetActive(true);
        }
    }

    // ����ѡ����� clone �б����Ѿ������ٵĶ���
    public void CleanUpClones()
    {
        clones.RemoveAll(clone => clone == null);
    }
}

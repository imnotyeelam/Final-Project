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
            clones.Add(clone);
    }

    public void MainBossDied()
    {
        mainBossDead = true;
        foreach (var clone in clones)
        {
            if (clone != null)
                clone.ForceKill(); // 下面我们会实现这个方法
        }
    }

    public bool IsMainBossDead()
    {
        return mainBossDead;
    }
}

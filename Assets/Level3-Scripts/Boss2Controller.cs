using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public Animator animator;
    public Transform[] summonPoints;
    public GameObject minionPrefab;
    public Transform player;

    private int currentWave = 0;
    private int totalWaves = 3;
    private List<GameObject> currentMinions = new List<GameObject>();
    private bool isSummoning = false;

    void Start()
    {
        // 起身后播放战斗Idle
        animator.SetTrigger("BattleIdle");
        Invoke(nameof(StartSummon), 1.5f); // 延迟 1.5s 后开始第一波召唤
    }

    void Update()
    {
        // 检查是否所有小兵都死亡，准备下一波
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), 2f); // 2秒后召唤下一波
            }
        }
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon"); // 播放 Victory_RF02_Anim
    }

    // 在 Victory_RF02_Anim 的 Animation Event 中调用这个
    public void SpawnMinions()
    {
        currentWave++;
        currentMinions.Clear();

        foreach (Transform point in summonPoints)
        {
            GameObject minion = Instantiate(minionPrefab, point.position, Quaternion.identity);
            minion.GetComponent<MinionAI>().SetTarget(player);
            currentMinions.Add(minion);
        }

        isSummoning = false;
    }

}

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
    private bool isAwake = false; // Boss 是否已经起身
    private bool playerInRange = false;

    void Start()
    {
        // 初始状态：lying down
        animator.Play("LyingDown"); // 确保 Animator 有这个状态
    }

    void Update()
    {
        // 检测条件：玩家收集 >= 2 块碎片 && 玩家在范围内 && Boss 还没起身
        if (!isAwake && playerInRange && PuzzleTracker.Instance.collectedPieces >= 2)
        {
            isAwake = true;
            animator.SetTrigger("GetUp");
        }

        // 检查是否所有小兵死亡，准备下一波
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), 2f); // 2秒后召唤下一波
            }
        }
    }

    // 触发器检测玩家是否进入 Boss 范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // **Animation Event：GetUp 播放结束后调用**
    public void OnGetUpFinished()
    {
        animator.SetTrigger("BattleIdle");
        Invoke(nameof(StartSummon), 2f); // 延迟 2s 后开始第一波召唤
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon"); // 播放 Victory_RF02_Anim
    }

    // 在 Summon 动画的 Animation Event 中调用
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

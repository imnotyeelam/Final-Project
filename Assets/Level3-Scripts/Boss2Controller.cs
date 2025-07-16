using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    [Header("Boss Settings")]
    public Animator animator;
    public Transform player;

    [Header("Summon Settings")]
    public Transform[] summonPoints;  // 小兵出生点
    public GameObject minionPrefab;   // 小兵预制体
    public int minionsPerWave = 5;    // 每波小兵数量
    public int totalWaves = 2;        // 总波数
    public float timeBetweenWaves = 2f; // 每波间隔时间

    private int currentWave = 0;
    private List<GameObject> currentMinions = new List<GameObject>();
    private bool isSummoning = false;

    [Header("Colliders")]
    public GameObject lyingColliderObject; // 拖入躺下状态的碰撞体空物体

    void Update()
    {
        // 检查是否所有小兵死亡，准备下一波
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), timeBetweenWaves);
            }
        }
    }

    /// <summary>
    /// 由 Trigger 调用，开始起身
    /// </summary>
    public void StartGetUpAnimation()
    {
        Debug.Log("Boss 开始起身！");
        animator.SetTrigger("GetUp");
    }

    /// <summary>
    /// GetUp 动画结束后，调用
    /// </summary>
    public void OnGetUpFinished()
    {
        Invoke(nameof(StartSummon), 1.5f);
    }

    /// <summary>
    /// 播放召唤动画
    /// </summary>
    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon");
    }

    /// <summary>
    /// Summon 动画事件调用 → 生成小兵
    /// </summary>
    public void SpawnMinions()
    {
        currentWave++;
        currentMinions.Clear();

        //Debug.Log("MinionPrefab: " + (minionPrefab != null));
        //Debug.Log("Player: " + (player != null));
        //Debug.Log("SummonPoints count: " + summonPoints.Length);
        Debug.Log($"开始第 {currentWave} 波召唤，共 {minionsPerWave} 个小兵");

        List<Transform> availablePoints = new List<Transform>(summonPoints);

        for (int i = 0; i < minionsPerWave; i++)
        {
            if (availablePoints.Count == 0) break; // 避免越界

            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[randomIndex];

            // 移除该点，防止重复
            availablePoints.RemoveAt(randomIndex);

            // 生成小兵
            GameObject minion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
            minion.GetComponent<MinionAI>().SetTarget(player);
            currentMinions.Add(minion);
        }

        isSummoning = false;
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = transform.position.y;
        transform.position = rootPosition;
        transform.rotation = animator.rootRotation;
    }

    public void DisableLyingCollider()
    {
        if (lyingColliderObject != null)
        {
            lyingColliderObject.SetActive(false);
            // 或者使用：lyingColliderObject.GetComponent<Collider>().enabled = false;
        }
    }
}

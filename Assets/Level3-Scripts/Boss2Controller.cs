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

    void Update()
    {
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), 2f); // 2秒后召唤下一波
            }
        }
    }

    // 用于 Trigger 调用
    public void StartGetUpAnimation()
    {
        Debug.Log("Boss 开始起身！");
        animator.SetTrigger("GetUp");
    }

    // 当 GetUp 动画结束后，动画事件调用这个
    public void OnGetUpFinished()
    {
        //animator.SetTrigger("BattleIdle");
        Invoke(nameof(StartSummon), 1.5f);
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon");
    }

    // Animation Event 放在 Summon 动画上
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

    private void OnAnimatorMove()
    {
        // 获取动画的 Root Motion
        Vector3 rootPosition = animator.rootPosition;

        // 锁定 Y 高度，让熊保持在地面
        rootPosition.y = transform.position.y;

        // 应用修改后的 Root Motion
        transform.position = rootPosition;

        // 如果要保留动画的旋转：
        transform.rotation = animator.rootRotation;
    }

}

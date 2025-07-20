using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss2Controller : MonoBehaviour
{
    [Header("Boss Settings")]
    public Animator animator;
    public Transform player;

    [Header("Summon Settings")]
    public Transform[] summonPoints;
    public GameObject minionPrefab;
    public int minionsPerWave = 5;
    public int totalWaves = 2;
    public float timeBetweenWaves = 2f;

    private int currentWave = 0;
    private List<GameObject> currentMinions = new List<GameObject>();
    private bool isSummoning = false;

    [Header("Colliders")]
    public GameObject lyingColliderObject;

    void Update()
    {
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);

            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                if (currentWave == 1)
                {
                    StartCoroutine(BossTalkSequence());
                }
                else
                {
                    Invoke(nameof(StartSummon), timeBetweenWaves);
                }
            }
        }
    }

    public void StartGetUpAnimation()
    {
        Debug.Log("Boss 开始起身！");
        animator.SetTrigger("GetUp");
    }

    public void OnGetUpFinished()
    {
        Invoke(nameof(StartSummon), 1.5f);
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon");
    }

    public void StartBossTalk()
    {
        animator.SetBool("isTalking", true);
    }

    public void EndBossTalk()
    {
        animator.SetBool("isTalking", false);
    }

    public void SpawnMinions()
    {
        currentWave++;
        currentMinions.Clear();

        Debug.Log($"开始第 {currentWave} 波召唤，共 {minionsPerWave} 个小兵");

        List<Transform> availablePoints = new List<Transform>(summonPoints);

        for (int i = 0; i < minionsPerWave; i++)
        {
            if (availablePoints.Count == 0) break;

            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[randomIndex];
            availablePoints.RemoveAt(randomIndex);

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
        }
    }

    IEnumerator BossTalkSequence()
    {
        isSummoning = true;

        // 不再切换摄像头，只播放动画 & 对白
        StartBossTalk();

        Debug.Log("It's all your fault for not treating me well!");

        yield return new WaitForSeconds(3f);

        EndBossTalk();

        isSummoning = false;
        StartSummon();
    }
}

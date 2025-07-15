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
    private bool isAwake = false; // Boss �Ƿ��Ѿ�����
    private bool playerInRange = false;

    void Start()
    {
        // ��ʼ״̬��lying down
        animator.Play("LyingDown"); // ȷ�� Animator �����״̬
    }

    void Update()
    {
        // �������������ռ� >= 2 ����Ƭ && ����ڷ�Χ�� && Boss ��û����
        if (!isAwake && playerInRange && PuzzleTracker.Instance.collectedPieces >= 2)
        {
            isAwake = true;
            animator.SetTrigger("GetUp");
        }

        // ����Ƿ�����С��������׼����һ��
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), 2f); // 2����ٻ���һ��
            }
        }
    }

    // �������������Ƿ���� Boss ��Χ
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

    // **Animation Event��GetUp ���Ž��������**
    public void OnGetUpFinished()
    {
        animator.SetTrigger("BattleIdle");
        Invoke(nameof(StartSummon), 2f); // �ӳ� 2s ��ʼ��һ���ٻ�
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon"); // ���� Victory_RF02_Anim
    }

    // �� Summon ������ Animation Event �е���
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

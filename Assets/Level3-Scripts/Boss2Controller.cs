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
        // ����󲥷�ս��Idle
        animator.SetTrigger("BattleIdle");
        Invoke(nameof(StartSummon), 1.5f); // �ӳ� 1.5s ��ʼ��һ���ٻ�
    }

    void Update()
    {
        // ����Ƿ�����С����������׼����һ��
        if (!isSummoning && currentMinions.Count > 0)
        {
            currentMinions.RemoveAll(m => m == null);
            if (currentMinions.Count == 0 && currentWave < totalWaves)
            {
                Invoke(nameof(StartSummon), 2f); // 2����ٻ���һ��
            }
        }
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon"); // ���� Victory_RF02_Anim
    }

    // �� Victory_RF02_Anim �� Animation Event �е������
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

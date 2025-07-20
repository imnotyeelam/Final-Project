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

    [Header("Chainsaw Settings")]
    public Animator chainsawAnimator; // 拖入电锯 Animator
    private AudioSource chainsawAudio;

    [Header("Audio Clips")]
    public AudioClip getUpSound;
    public AudioClip summonSound;
    public AudioClip chainsawClip; // 电锯音效

    private AudioSource audioSource;

    void Start()
    {
        // 主 AudioSource（播放一次性音效：起身、召唤）
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 电锯 AudioSource（循环播放，3D）
        if (chainsawAnimator != null)
        {
            chainsawAudio = chainsawAnimator.GetComponent<AudioSource>();
            if (chainsawAudio == null)
            {
                chainsawAudio = chainsawAnimator.gameObject.AddComponent<AudioSource>();
            }

            chainsawAudio.clip = chainsawClip;
            chainsawAudio.loop = true;
            chainsawAudio.spatialBlend = 1f; // 3D音效
            chainsawAudio.minDistance = 3f;
            chainsawAudio.maxDistance = 20f;
            chainsawAudio.playOnAwake = false;
        }
    }

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

    /// <summary>
    /// 熊倒下 → 停止电锯
    /// </summary>
    public void OnLyingDownStart()
    {
        chainsawAnimator.SetBool("isActive", false);
        if (chainsawAudio.isPlaying) chainsawAudio.Stop();
    }

    /// <summary>
    /// 播放起身动画
    /// </summary>
    public void StartGetUpAnimation()
    {
        Debug.Log("Boss 开始起身！");
        animator.SetTrigger("GetUp");
    }

    /// <summary>
    /// 起身音效（动画事件调用）
    /// </summary>
    public void PlayGetUpSound()
    {
        if (getUpSound != null)
            audioSource.PlayOneShot(getUpSound);
    }

    /// <summary>
    /// 起身完成 → 激活电锯
    /// </summary>
    public void OnGetUpFinished()
    {
        Invoke(nameof(StartSummon), 1.5f);

        chainsawAnimator.SetBool("isActive", true);

        if (!chainsawAudio.isPlaying && chainsawClip != null)
            chainsawAudio.Play();
    }

    void StartSummon()
    {
        isSummoning = true;
        animator.SetTrigger("Summon");
    }

    /// <summary>
    /// 召唤音效（动画事件调用）
    /// </summary>
    public void PlaySummonSound()
    {
        if (summonSound != null)
            audioSource.PlayOneShot(summonSound);
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

        StartBossTalk();
        Debug.Log("It's all your fault for not treating me well!");
        yield return new WaitForSeconds(3f);
        EndBossTalk();

        isSummoning = false;
        StartSummon();
    }
}

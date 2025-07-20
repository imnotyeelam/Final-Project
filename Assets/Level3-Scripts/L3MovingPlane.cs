using UnityEngine;
using System.Collections;

public class L3MovingPlane : MonoBehaviour
{
    public Transform pointA;          // 起点
    public Transform pointB;          // 终点
    public float moveSpeed = 5f;      // 移动速度
    public float rotateSpeed = 2f;    // 旋转速度
    public float waitTime = 2f;       // 到达后停顿
    public float extraWaitAfterRotate = 1f; // 转完180°后的额外停顿

    [Header("Audio Settings")]
    public AudioClip flightSound;     // 飞机飞行音效
    private AudioSource audioSource;

    private Transform targetPoint;
    private bool isSwitching = false; // 防止重复切换

    private bool canAttach = true;
    private void Start()
    {
        targetPoint = pointB;

        // 初始化音频
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = flightSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;

        // **设置为3D音效**
        audioSource.spatialBlend = 1f; // 完全3D
        audioSource.minDistance = 20f;  // 近距离最大音量
        audioSource.maxDistance = 50f; // 超过50米就听不到
        audioSource.rolloffMode = AudioRolloffMode.Linear; // 线性衰减
    }


    private void Update()
    {
        if (isSwitching)
        {
            StopFlightSound();
            return; // 停顿或旋转时暂停移动
        }

        // 飞机移动
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // **播放飞行音效**
        PlayFlightSound();

        // 判断是否到达目标点
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f && !isSwitching)
        {
            StartCoroutine(SwitchTarget());
        }
    }

    IEnumerator SwitchTarget()
    {
        isSwitching = true;

        StopFlightSound();

        yield return new WaitForSeconds(waitTime);

        Transform nextPoint = (targetPoint == pointB) ? pointA : pointB;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);

        // 平滑旋转
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(extraWaitAfterRotate);

        targetPoint = nextPoint;
        isSwitching = false;
    }

    void PlayFlightSound()
    {
        if (!audioSource.isPlaying && flightSound != null)
        {
            audioSource.Play();
        }
    }

    void StopFlightSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canAttach) return;

        if (other.CompareTag("Player"))
        {
            PlayerController1.instance.EnterPlatform(transform);
        }
    }

    public void TemporarilyDisableAttach(float delay)
    {
        StartCoroutine(DisableAttachRoutine(delay));
    }

    IEnumerator DisableAttachRoutine(float delay)
    {
        canAttach = false;
        yield return new WaitForSeconds(delay);
        canAttach = true;
    }
}

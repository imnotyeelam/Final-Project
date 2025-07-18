using UnityEngine;

public class L2CheckpointManager : MonoBehaviour
{
    public static Vector3 lastCheckpointPos = Vector3.zero; // 当前激活的 checkpoint
    public Transform player;  // 只有一个脚本需要指定 player，其它 checkpoint 不需要

    private void Start()
    {
        // 游戏开始时尝试从 PlayerPrefs 读取存档位置
        if (lastCheckpointPos == Vector3.zero)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX", float.NaN);
            float y = PlayerPrefs.GetFloat("CheckpointY", float.NaN);
            float z = PlayerPrefs.GetFloat("CheckpointZ", float.NaN);

            if (!float.IsNaN(x))
                lastCheckpointPos = new Vector3(x, y, z);
        }

        // 第一次启动时把玩家送到存档点
        if (player != null && lastCheckpointPos != Vector3.zero)
        {
            player.position = lastCheckpointPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 保存该 checkpoint 位置
            lastCheckpointPos = transform.position;
            PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
            PlayerPrefs.SetFloat("CheckpointZ", transform.position.z);
            PlayerPrefs.Save();

            Debug.Log("Checkpoint saved at: " + transform.position);
        }
    }

    // 可以在其他脚本中调用此函数进行复活
    public static void RespawnPlayer(Transform player)
    {
        if (lastCheckpointPos != Vector3.zero)
        {
            player.position = lastCheckpointPos;
        }
    }
}

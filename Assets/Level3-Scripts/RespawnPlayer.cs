using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class L3RespawnManager : MonoBehaviour
{
    public float respawnDelay = 2f;  // 死亡后延迟

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // 禁用玩家操作
        PlayerController1.instance.enabled = false;
        PlayerController1.instance.charCon.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // 获取当前 checkpoint
        string key = SceneManager.GetActiveScene().name + "_cp";
        Vector3 respawnPos = Vector3.zero;
        if (PlayerPrefs.HasKey(key) && L3CheckpointManager.checkpointDict.ContainsKey(PlayerPrefs.GetString(key)))
        {
            respawnPos = L3CheckpointManager.checkpointDict[PlayerPrefs.GetString(key)].position;
        }
        else
        {
            respawnPos = PlayerController1.instance.transform.position; // fallback
        }

        // 传送玩家
        PlayerController1.instance.transform.position = respawnPos;

        // 恢复控制
        PlayerController1.instance.charCon.enabled = true;
        PlayerController1.instance.enabled = true;
    }
}

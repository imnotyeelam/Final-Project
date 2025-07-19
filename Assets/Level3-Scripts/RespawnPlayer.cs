using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class L3RespawnManager : MonoBehaviour
{
    public float respawnDelay = 2f;  // �������ӳ�

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // ������Ҳ���
        PlayerController1.instance.enabled = false;
        PlayerController1.instance.charCon.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // ��ȡ��ǰ checkpoint
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

        // �������
        PlayerController1.instance.transform.position = respawnPos;

        // �ָ�����
        PlayerController1.instance.charCon.enabled = true;
        PlayerController1.instance.enabled = true;
    }
}

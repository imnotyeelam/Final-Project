using UnityEngine;

public class L2CheckpointManager : MonoBehaviour
{
    public static Vector3 lastCheckpointPos = Vector3.zero; // ��ǰ����� checkpoint
    public Transform player;  // ֻ��һ���ű���Ҫָ�� player������ checkpoint ����Ҫ

    private void Start()
    {
        // ��Ϸ��ʼʱ���Դ� PlayerPrefs ��ȡ�浵λ��
        if (lastCheckpointPos == Vector3.zero)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX", float.NaN);
            float y = PlayerPrefs.GetFloat("CheckpointY", float.NaN);
            float z = PlayerPrefs.GetFloat("CheckpointZ", float.NaN);

            if (!float.IsNaN(x))
                lastCheckpointPos = new Vector3(x, y, z);
        }

        // ��һ������ʱ������͵��浵��
        if (player != null && lastCheckpointPos != Vector3.zero)
        {
            player.position = lastCheckpointPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ����� checkpoint λ��
            lastCheckpointPos = transform.position;
            PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
            PlayerPrefs.SetFloat("CheckpointZ", transform.position.z);
            PlayerPrefs.Save();

            Debug.Log("Checkpoint saved at: " + transform.position);
        }
    }

    // �����������ű��е��ô˺������и���
    public static void RespawnPlayer(Transform player)
    {
        if (lastCheckpointPos != Vector3.zero)
        {
            player.position = lastCheckpointPos;
        }
    }
}

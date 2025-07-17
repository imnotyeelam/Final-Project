using UnityEngine;
using TMPro; // ������õ��� TextMeshPro

public class ShowHintTrigger : MonoBehaviour
{
    public GameObject hintUI; // ���� UI ���壨���� Text �����

    void Start()
    {
        if (hintUI != null)
        {
            hintUI.SetActive(false); // ��ʼ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hintUI != null)
        {
            hintUI.SetActive(true); // ��ʾԭ���� Text ��������õ�����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hintUI != null)
        {
            hintUI.SetActive(false); // ������ʾ
        }
    }
}

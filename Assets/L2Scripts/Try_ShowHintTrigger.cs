using UnityEngine;
using TMPro; // 如果你用的是 TextMeshPro

public class Try_ShowHintTrigger : MonoBehaviour
{
    public GameObject hintUI; // 拖入 UI 物体（带有 Text 组件）

    void Start()
    {
        if (hintUI != null)
        {
            hintUI.SetActive(false); // 初始隐藏
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hintUI != null)
        {
            hintUI.SetActive(true); // 显示原本在 Text 组件中设置的文字
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hintUI != null)
        {
            hintUI.SetActive(false); // 隐藏提示
        }
    }
}

using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [Header("Dialogue Content")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Portraits")]
    public Sprite[] dialoguePortraits;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            DialogueUI.Instance.StartDialogue(dialogueLines, dialoguePortraits);
            hasTriggered = true;
        }
    }
}

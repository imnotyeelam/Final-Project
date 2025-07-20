using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text speakerNameText;
    public Image portraitImage;
    public GameObject nextIcon;
    public GameObject[] uiElementsToHide;

    [Header("Speaker Database")]
    public SpeakerData[] speakers;

    private int currentLineIndex = 0;
    private string[] lines;
    private Sprite[] portraits;

    private bool isTyping = false;
    private string fullText = "";

    private SimpleFPSMovement player;
    public bool IsDialogueActive => dialoguePanel.activeSelf;

    void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    void Start()
    {
        player = FindObjectOfType<SimpleFPSMovement>();
    }

    public void StartDialogue(string[] dialogueLines, Sprite[] dialoguePortraits)
    {
        lines = dialogueLines;
        portraits = dialoguePortraits;
        currentLineIndex = 0;

        dialoguePanel.SetActive(true);
        DisablePlayerControl(true);
        ShowLine();
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return)) // Enter key
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = fullText;
                isTyping = false;
                nextIcon.SetActive(true);
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex >= lines.Length)
                    EndDialogue();
                else
                    ShowLine();
            }
        }
    }

    void ShowLine()
    {
        fullText = lines[currentLineIndex];
        dialogueText.text = "";
        nextIcon.SetActive(false);

        Sprite currentPortrait = portraits[currentLineIndex];
        portraitImage.sprite = currentPortrait;

        SpeakerData matchedSpeaker = speakers.FirstOrDefault(s => s.portrait == currentPortrait);
        if (matchedSpeaker != null)
        {
            speakerNameText.text = matchedSpeaker.speakerName;
        }
        else
        {
            speakerNameText.text = "";
        }

        StartCoroutine(TypeText(fullText));
    }

    System.Collections.IEnumerator TypeText(string text)
    {
        isTyping = true;
        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
        nextIcon.SetActive(true);
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        DisablePlayerControl(false);
    }

    void DisablePlayerControl(bool disable)
    {
        if (player != null)
            player.canMove = !disable;

        foreach (var ui in uiElementsToHide)
        {
            if (ui != null)
                ui.SetActive(!disable);
        }
    }
}

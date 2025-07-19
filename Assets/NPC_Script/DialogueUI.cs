using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Text nameText;
    public Text dialogueText;
    public Image portraitImage;

    [Header("Speaker Database")]
    public SpeakerData[] speakerDatabase;

    private string[] lines;
    private Sprite[] portraits;
    private int currentIndex = 0;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    /// <summary>
    /// Start dialogue with lines + portraits
    /// </summary>
    public void StartDialogue(string[] dialogueLines, Sprite[] dialoguePortraits)
    {
        if (dialogueLines.Length != dialoguePortraits.Length)
        {
            Debug.LogWarning("Lines count and portraits count mismatch!");
        }

        lines = dialogueLines;
        portraits = dialoguePortraits;
        currentIndex = 0;
        isActive = true;

        dialoguePanel.SetActive(true);
        ShowLine(currentIndex);
    }

    void ShowNextLine()
    {
        currentIndex++;
        if (currentIndex < lines.Length)
        {
            ShowLine(currentIndex);
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowLine(int index)
    {
        dialogueText.text = lines[index];
        portraitImage.sprite = portraits[index];

        string foundName = GetNameByPortrait(portraits[index]);
        nameText.text = foundName;
    }

    string GetNameByPortrait(Sprite portrait)
    {
        var speaker = speakerDatabase.FirstOrDefault(s => s.portrait == portrait);
        return speaker != null ? speaker.speakerName : "???";
    }

    void EndDialogue()
    {
        isActive = false;
        dialoguePanel.SetActive(false);
    }
}

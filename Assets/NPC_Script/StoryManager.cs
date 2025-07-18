using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class StoryFrame
{
    public Sprite image;
    [TextArea] public string text;
}

public class StoryManager : MonoBehaviour
{
    [Header("UI References")]
    public Image storyImage;
    public Text dialogueText;
    public GameObject nextIcon;
    public GameObject dialoguePanel; // drag your existing dialogue panel here

    [Header("Typewriter Effect")]
    public float typeSpeed = 0.03f;

    [Header("Story Frames")]
    public StoryFrame[] frames;

    private int currentIndex = 0;
    private bool isTyping = false;
    private string fullText = "";

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowFrame(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
                NextFrame();
            }
        }
    }

    void ShowFrame(int index)
    {
        if (index >= frames.Length)
        {
            EndStory();
            return;
        }

        storyImage.sprite = frames[index].image;

        fullText = frames[index].text;
        dialogueText.text = "";
        nextIcon.SetActive(false);

        StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
        nextIcon.SetActive(true);
    }

    void NextFrame()
    {
        currentIndex++;
        ShowFrame(currentIndex);
    }

    void EndStory()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("NPC_UI");
    }

    public void SkipAll()
    {
        StopAllCoroutines();
        EndStory();
    }
}

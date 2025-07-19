using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public GameObject dialoguePanel;

    [Header("Story Settings")]
    public float typeSpeed = 0.03f;
    public StoryFrame[] frames;

    [Header("Transition")]
    public Image blackScreen;         // full-screen black image
    public float fadeDuration = 1f;   // fade in/out time
    public string nextSceneName = "NPC_UI";

    private int currentIndex = 0;
    private bool isTyping = false;
    private string fullText = "";

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
            StartCoroutine(StartSequence());
        }
        else
        {
            // no blackscreen -> just show first frame
            StartStory();
        }
    }

    IEnumerator StartSequence()
    {
        // ensure fully black
        Color c = blackScreen.color;
        c.a = 1f;
        blackScreen.color = c;

        yield return StartCoroutine(FadeFromBlack());
        StartStory();
    }

    void StartStory()
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

        if (blackScreen != null)
            StartCoroutine(FadeToBlack());
        else
            SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color c = blackScreen.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            blackScreen.color = c;
            yield return null;
        }
        blackScreen.gameObject.SetActive(false);
    }

    IEnumerator FadeToBlack()
    {
        blackScreen.gameObject.SetActive(true);

        Color c = blackScreen.color;
        c.a = 0f;
        blackScreen.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            blackScreen.color = c;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}

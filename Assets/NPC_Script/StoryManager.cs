using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Typewriter Effect")]
    public float typeSpeed = 0.03f;

    private StoryFrame[] storyFrames;
    private int currentIndex = 0;
    private bool isTyping = false;
    private string fullText = "";

    public void StartStory(StoryFrame[] frames)
    {
        storyFrames = frames;
        currentIndex = 0;
        ShowFrame(currentIndex);
    }

    void Update()
    {
        if (storyFrames == null) return;

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
        if (index >= storyFrames.Length)
        {
            EndStory();
            return;
        }

        storyImage.sprite = storyFrames[index].image;

        fullText = storyFrames[index].text;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

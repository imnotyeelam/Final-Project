using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{
    public GameObject mainMenuUI;

    void Start()
    {
        // Ensure timescale is normal in menu (in case returning from pause)
        Time.timeScale = 1f;

        // Show Main Menu UI
        mainMenuUI.SetActive(true);

        // Cursor should be visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("NPC_Scenes/Storyline"); // Your actual game scene name
    }

    public void ExitGame()
    {
        Debug.Log("Exit clicked");
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ===== Open Pause Menu =====
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        settingsPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    // ===== Back =====
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    // ===== Open Settings =====
    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // ===== Settings Back =====
    public void BackToPause()
    {
        settingsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }

    // ===== Back to MainMenu =====
    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }
}
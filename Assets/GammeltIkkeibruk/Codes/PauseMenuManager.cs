using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    
    public GameObject pauseMenuPanel;
    public GameObject InGameoptionsPanel;

    private bool isPaused = false;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        InGameoptionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        pauseMenuPanel.SetActive(false);
        InGameoptionsPanel.SetActive(true);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; 
        ShowPauseMenu();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; 
        pauseMenuPanel.SetActive(false);

    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);

    }
}

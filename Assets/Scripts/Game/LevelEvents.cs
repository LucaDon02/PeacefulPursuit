using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public GameObject gamePausedPanel;
    public Button pauseButton; 

    private void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        if (PlayerManager.gameOver)
            pauseButton.interactable = false;

        if (PlayerManager.gameOver)
            return;


        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (PlayerManager.isGamePaused)
        {
            ResumeGame();
            gamePausedPanel.SetActive(false);
        }
        else
        {
            PauseGame();
            gamePausedPanel.SetActive(true);
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void PauseGame()
    {
        if (PlayerManager.isGamePaused || PlayerManager.gameOver) return;
        Time.timeScale = 0;
        PlayerManager.isGamePaused = true;
    }
    
    public void ResumeGame()
    {
        if (!PlayerManager.isGamePaused) return;
        Time.timeScale = 1;
        PlayerManager.isGamePaused = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }


}

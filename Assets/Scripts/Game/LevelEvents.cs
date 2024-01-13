using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class LevelEvents : MonoBehaviour
    {
        public GameObject gamePausedPanel;
        public Button pauseButton;
        public QuestionManager questionManager;

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
            questionManager.ResetQuestionUI();
        }
    
        public void ResumeGame()
        {
            if (!PlayerManager.isGamePaused) return;
            Time.timeScale = 1;
            PlayerManager.isGamePaused = false;
            questionManager.SetQuestionUI();
        }
    
    }
}

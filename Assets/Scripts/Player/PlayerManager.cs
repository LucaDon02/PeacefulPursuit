using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject newRecordPanel;
    
    public TextMeshProUGUI newRecordText;

    public static bool isGamePaused;
    
    public Player player1;
    public Player player2;

    private float countdownTime = 3.5f;
    public TextMeshProUGUI countdownTextPlayer1;
    public TextMeshProUGUI countdownTextPlayer2;

    void Start()
    {
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused = false;
    }

    void Update()
    {
        //Game Over
        if (gameOver)
        {
            Time.timeScale = 0;
            var highestScore = player1.score > player2.score ? player1.score : player2.score;
            if (highestScore > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordPanel.SetActive(true);
                newRecordText.text = "New \nRecord\n" + highestScore;
                PlayerPrefs.SetInt("HighScore", highestScore);
            }

            gameOverPanel.SetActive(true);
            Destroy(gameObject);
        }

        //Start Game
        if (!isGameStarted)
        {
            countdownTime -= 1 * Time.deltaTime;
            countdownTextPlayer1.text = countdownTime.ToString("0");
            countdownTextPlayer2.text = countdownTime.ToString("0");

            if (countdownTime <= 0)
            {
                countdownTextPlayer1.gameObject.SetActive(false);
                countdownTextPlayer2.gameObject.SetActive(false);
                isGameStarted = true;
            }
        }
    }

    public void AddPoint(string playerName){ (playerName == "Player1" ? player1 : player2).score++; }
}

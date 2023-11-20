using System;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [NonSerialized] public static bool gameOver;
    
    [NonSerialized] public static bool gameOverPlayer1;
    public GameObject gameOverPanelPlayer1;
    public GameObject newRecordPanelPlayer1;
    public TextMeshProUGUI newRecordTextPlayer1;
    
    [NonSerialized] public static bool gameOverPlayer2;
    public GameObject gameOverPanelPlayer2;
    public GameObject newRecordPanelPlayer2;
    public TextMeshProUGUI newRecordTextPlayer2;

    public static bool isGameStarted;
    public static bool isGamePaused;
    
    public Player player1;
    public Player player2;

    private float countdownTime = 3.5f;
    public TextMeshProUGUI countdownTextPlayer1;
    public TextMeshProUGUI countdownTextPlayer2;

    private void Start()
    {
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused = false;
    }

    private void Update()
    {
        //Game Over
        if (gameOverPlayer1)
        {
            if (gameOverPlayer2) gameOver = true;
            if (player1.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordPanelPlayer1.SetActive(true);
                newRecordTextPlayer1.text = "New \nRecord\n" + player1.score;
                PlayerPrefs.SetInt("HighScore", player1.score);
            }

            gameOverPanelPlayer1.SetActive(true);
            Destroy(player1);
        }
        
        if (gameOverPlayer2)
        {
            if (gameOverPlayer1) gameOver = true;
            if (player2.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordPanelPlayer2.SetActive(true);
                newRecordTextPlayer2.text = "New \nRecord\n" + player2.score;
                PlayerPrefs.SetInt("HighScore", player2.score);
            }

            gameOverPanelPlayer2.SetActive(true);
            Destroy(player2);
        }
        
        if (gameOver)
        {
            Time.timeScale = 0;
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

    public void AddPoint(string playerName, int amount = 1){ (playerName == "Player1" ? player1 : player2).score += amount; }

    public void PearPickup(string playerName){ (playerName == "Player1" ? player1 : player2).buff += 1; }

    public void ApplePickup(string playerName){ (playerName == "Player1" ? player2 : player1).debuff += 1; }

    public void PlayerHitObstacle(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player1.buff = 0;
            player2.debuff = 0;
        }
        else
        {
            player2.buff = 0;
            player1.debuff = 0;
        }
    }
}

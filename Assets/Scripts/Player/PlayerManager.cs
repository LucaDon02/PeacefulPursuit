using System;
using UnityEngine;
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
    
    public TextMeshProUGUI buffTextPlayer1;
    public TextMeshProUGUI debuffTextPlayer1;
    [NonSerialized] public float buffMultiplierPlayer1 = 1;
    [NonSerialized] public float debuffMultiplierPlayer1 = 1;
    
    public TextMeshProUGUI buffTextPlayer2;
    public TextMeshProUGUI debuffTextPlayer2;
    [NonSerialized] public float buffMultiplierPlayer2 = 1;
    [NonSerialized] public float debuffMultiplierPlayer2 = 1;

    private void Start()
    {
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused = false;
    }

    private void Update()
    {
        buffTextPlayer1.text = buffMultiplierPlayer1.ToString("0.0");
        debuffTextPlayer1.text = debuffMultiplierPlayer1.ToString("0.0");
        buffTextPlayer2.text = buffMultiplierPlayer2.ToString("0.0");
        debuffTextPlayer2.text = debuffMultiplierPlayer2.ToString("0.0");
        
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

    public void AddPoint(string playerName, int amount = 1){ (playerName == "Player1" ? player1 : player2).score += amount; }
}

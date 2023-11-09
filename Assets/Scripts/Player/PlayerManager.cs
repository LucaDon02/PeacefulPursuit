using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject newRecordPanel;

    public static int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI newRecordText;

    public static bool isGamePaused;
    public GameObject[] characterPrefabs;

    private void Awake()
    {
        var index = PlayerPrefs.GetInt("SelectedCharacter");
        Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
    }

    void Start()
    {
        score = 0;
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused = false;
    }

    void Update()
    {
        //Update UI
        scoreText.text = score.ToString();

        //Game Over
        if (gameOver)
        {
            Time.timeScale = 0;
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordPanel.SetActive(true);
                newRecordText.text = "New \nRecord\n" + score;
                PlayerPrefs.SetInt("HighScore", score);
            }

            gameOverPanel.SetActive(true);
            Destroy(gameObject);
        }

        //Start Game
        if (!isGameStarted)
        {
            isGameStarted = true;
        }
    }
}

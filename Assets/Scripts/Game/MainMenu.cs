using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        highScoreText.text = "High Score\n" + PlayerPrefs.GetInt("HighScore", 0);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

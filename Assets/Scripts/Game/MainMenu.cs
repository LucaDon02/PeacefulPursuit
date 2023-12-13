using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public int characterIndexPlayer1;//0:Wheel, 1:Amy, 2:Michelle ...
    public int characterIndexPlayer2;//0:Wheel, 1:Amy, 2:Michelle ...
    
    public GameObject[] charactersPlayer1;
    public GameObject[] charactersPlayer2;

    private bool player1Ready;
    private bool player2Ready;

    public TextMeshProUGUI highScoreText;
    public GameObject cms;

    private void Start()
    {
        Time.timeScale = 1;
        
        characterIndexPlayer1 = PlayerPrefs.GetInt("SelectedCharacterPlayer1", 0);
        foreach (var ch in charactersPlayer1) ch.SetActive(false);
        charactersPlayer1[characterIndexPlayer1].SetActive(true);
        
        characterIndexPlayer2 = PlayerPrefs.GetInt("SelectedCharacterPlayer2", 0);
        foreach (var ch in charactersPlayer2) ch.SetActive(false);
        charactersPlayer2[characterIndexPlayer2].SetActive(true);
    }
    
    private void Update()
    {
        var highscorePlayer1 = JSONManager.scores.scoresPlayer1.Max();
        var highscorePlayer2 = JSONManager.scores.scoresPlayer2.Max();
        var highscore = highscorePlayer1 > highscorePlayer2 ? highscorePlayer1 : highscorePlayer2;
        highScoreText.text = "High Score\n" + highscore;
    }

    public void ChangeNextCharacter(GameObject parentObject)
    {
        switch (parentObject.name)
        {
            case "MainMenu Player1":
                charactersPlayer1[characterIndexPlayer1].SetActive(false);

                characterIndexPlayer1++;
                if (characterIndexPlayer1 == charactersPlayer1.Length) characterIndexPlayer1 = 0;

                charactersPlayer1[characterIndexPlayer1].SetActive(true);
                PlayerPrefs.SetInt("SelectedCharacterPlayer1", characterIndexPlayer1);
                break;
            case "MainMenu Player2":
                charactersPlayer2[characterIndexPlayer2].SetActive(false);

                characterIndexPlayer2++;
                if (characterIndexPlayer2 == charactersPlayer2.Length) characterIndexPlayer2 = 0;

                charactersPlayer2[characterIndexPlayer2].SetActive(true);
                PlayerPrefs.SetInt("SelectedCharacterPlayer2", characterIndexPlayer2);
                break;
        }
    }

    public void ChangePreviousCharacter(GameObject parentObject)
    {
        switch (parentObject.name)
        {
            case "MainMenu Player1":
                charactersPlayer1[characterIndexPlayer1].SetActive(false);

                characterIndexPlayer1--;
                if (characterIndexPlayer1 == -1) characterIndexPlayer1 = charactersPlayer1.Length - 1;

                charactersPlayer1[characterIndexPlayer1].SetActive(true);
                PlayerPrefs.SetInt("SelectedCharacterPlayer1", characterIndexPlayer1);
                break;
            case "MainMenu Player2":
                charactersPlayer2[characterIndexPlayer2].SetActive(false);

                characterIndexPlayer2--;
                if (characterIndexPlayer2 == -1) characterIndexPlayer2 = charactersPlayer2.Length - 1;

                charactersPlayer2[characterIndexPlayer2].SetActive(true);
                PlayerPrefs.SetInt("SelectedCharacterPlayer2", characterIndexPlayer2);
                break;
        }
    }

    public void Ready(Button button)
    {
        button.interactable = false;

        switch (button.transform.parent.name)
        {
            case "MainMenu Player1":
                player1Ready = true;
                break;
            case "MainMenu Player2":
                player2Ready = true;
                break;
        }
        
        if (player1Ready && player2Ready) SceneManager.LoadScene("Level");
    }

    public void OpenCMS() { cms.SetActive(true); }
}

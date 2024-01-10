using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [NonSerialized] public static bool gameOver;
    public GameObject menuButton;
    public GameObject pauseButton;
    
    public GameObject gameOverPanelPlayer1;
    public GameObject gameOverPanelPlayer2;
    
    public ScrollRect gameOverPanelScrollRectPlayer1;
    public ScrollRect gameOverPanelScrollRectPlayer2;
    
    public GameObject gameOverPanelContainerPlayer1;
    public GameObject gameOverPanelContainerPlayer2;
    
    public Scrollbar gameOverPanelScrollbarPlayer1;
    public Scrollbar gameOverPanelScrollbarPlayer2;
    
    public GameObject newRecordPanelPlayer1;
    public GameObject newRecordPanelPlayer2;
    
    public TextMeshProUGUI newRecordTextPlayer1;
    public TextMeshProUGUI newRecordTextPlayer2;
    
    public DamageAnimation damageAnimationPlayer1;
    public DamageAnimation damageAnimationPlayer2;

    public static bool isGameStarted;
    public static bool isGamePaused;
    
    public Player player1;
    public Player player2;

    private float countdownTime = 3.5f;
    public TextMeshProUGUI countdownTextPlayer1;
    public TextMeshProUGUI countdownTextPlayer2;

    public float changeTimerDuration = 45.49f;
    public float intervalChangeTimerDuration = 10.49f;
    public TextMeshProUGUI timerText;
    public GameObject changePlayerPanel;
    private float timer;
    private bool isChanging;
    private float previousTimeScale;

    public int defaultCorrectQuestionBonus = 25;

    private void Start()
    {
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused = false;
        timer = changeTimerDuration;
        
        foreach (var playerInput in GameObject.FindGameObjectsWithTag("PlayerInput"))
        {
            var inputHandler = playerInput.GetComponent<PlayerInputHandler>();
            inputHandler.playerController = inputHandler.index switch
            {
                0 => player1.player.GetComponent<PlayerController>(),
                1 => player2.player.GetComponent<PlayerController>(),
                _ => throw new ArgumentOutOfRangeException()
            };
            inputHandler.playerManager = this;
            inputHandler.isGameStarted = true;
        }
    }

    private void Update()
    {
        //Game Over
        if (gameOver)
        {
            if (player1.score == player2.score)
            {
                if (player1.score > JsonManager.GetHighScore())
                {
                    newRecordPanelPlayer1.SetActive(true);
                    newRecordTextPlayer1.text = "Nieuw Record!\n" + player1.score;

                    newRecordPanelPlayer2.SetActive(true);
                    newRecordTextPlayer2.text = "Nieuw Record!\n" + player2.score;
                }
            }
            else
            {
                if (player1.score > player2.score && player1.score > JsonManager.GetHighScore())
                {
                    newRecordPanelPlayer1.SetActive(true);
                    newRecordTextPlayer1.text = "Nieuw Record!\n" + player1.score;
                }
                else if (player2.score > JsonManager.GetHighScore())
                {
                    newRecordPanelPlayer2.SetActive(true);
                    newRecordTextPlayer2.text = "Nieuw Record!\n" + player2.score;
                }
            }
            timerText.gameObject.transform.parent.gameObject.SetActive(false);
            gameOverPanelScrollRectPlayer1.scrollSensitivity = gameOverPanelContainerPlayer1.transform.childCount / 4f;
            gameOverPanelPlayer1.SetActive(true);
            gameOverPanelPlayer2.SetActive(true);
            pauseButton.SetActive(false);
            menuButton.SetActive(true);
            
            Destroy(player1);
            Destroy(player2);

            JsonManager.AddScores(player1.score, player2.score);

            Time.timeScale = 0;
            Destroy(gameObject);
        }
        
        //Timer
        if (isGameStarted && !isGamePaused) timer -= Time.unscaledDeltaTime;
        
        if (timer <= 0)
        {
            timer = isChanging ? changeTimerDuration : intervalChangeTimerDuration;
            
            changePlayerPanel.SetActive(!isChanging);
            if (!isChanging) previousTimeScale = Time.timeScale;
            Time.timeScale = isChanging ? previousTimeScale : 0;
            
            isChanging = !isChanging;
            if (isChanging) {
                FindObjectOfType<AudioManager>().PauseSound("MainTheme");
                FindObjectOfType<AudioManager>().PlaySound("changeController");
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("MainTheme");
            }
        }

        timerText.text = timer.ToString("00");

        //Start Game
        if (!isGameStarted)
        {
            countdownTime -= Time.deltaTime;
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
            damageAnimationPlayer1.start = true;
            player1.buff = 0;
            player2.debuff = 0;
        }
        else
        {
            damageAnimationPlayer2.start = true;
            player2.buff = 0;
            player1.debuff = 0;
        }
    }
    
    public void CorrectQuestion(string playerName)
    {
        var player = playerName == "Player1" ? player1 : player2;
        player.score += (int)(defaultCorrectQuestionBonus * ((player.buff - player.debuff) / 10f + 1) + 0.5);
    }

    public void ScrollGameOverContainer(bool isPlayer1, bool isUp)
    {
        if (isPlayer1)
        {
            if ((gameOverPanelScrollbarPlayer1.value < 0.01 && !isUp) || (gameOverPanelScrollbarPlayer1.value > 0.99 && isUp)) return;
            var value = 1f / (gameOverPanelContainerPlayer1.transform.childCount / 2f);

            gameOverPanelScrollbarPlayer1.value = isUp
                ? gameOverPanelScrollbarPlayer1.value + value
                : gameOverPanelScrollbarPlayer1.value - value;
        }
        else
        {
            if ((gameOverPanelScrollbarPlayer2.value < 0.01 && isUp) || (gameOverPanelScrollbarPlayer2.value > 0.99 && !isUp)) return;
            var value = 1f / (gameOverPanelContainerPlayer2.transform.childCount / 2f);

            gameOverPanelScrollbarPlayer2.value = isUp
                ? gameOverPanelScrollbarPlayer2.value + value
                : gameOverPanelScrollbarPlayer2.value - value;
        }
    }
}

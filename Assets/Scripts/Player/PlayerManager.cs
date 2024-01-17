using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    /// <summary>
    /// This class handles Powerups, Debuffs, Questions, Displaying of correct answers and more
    /// Of the two players
    /// </summary>
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

        public TextMeshProUGUI scoreTextPlayer1;
        public TextMeshProUGUI scoreTextPlayer2;
    
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
        [NonSerialized] public static bool isChanging;
        private float previousTimeScale;

        public int defaultCorrectQuestionBonus = 25;
        public Image checkmarkPlayer1;
        public Image checkmarkPlayer2;
        public Image crossPlayer1;
        public Image crossPlayer2;

        public double secondsFading;
        private double currentFadingTime = 0;
        public bool isFlashing;
        private bool isFadingIn = true;
        private bool isCorrectAnswerPlayer1; 
        private bool isCorrectAnswerPlayer2;

        // constants
        private const string ThemeToPlay = "MainTheme";
        private const string NewRecordMessage = "Nieuw Record!\n";
        private const string CorrectAnswerSoundTitle = "correctAnswer";
        private const string WrongAnswerSoundTitle = "wrongAnswer";
    
        [SerializeField] private QuestionManager questionManager;

        private void Start()
        {
            Time.timeScale = 1;
            gameOver = isGameStarted = isGamePaused = false;
            timer = changeTimerDuration;
        
            foreach (var playerInput in GameObject.FindGameObjectsWithTag("PlayerInput"))
            {
                PlayerInputHandler inputHandler = playerInput.GetComponent<PlayerInputHandler>();
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
                        newRecordTextPlayer1.text = NewRecordMessage + player1.score;

                        newRecordPanelPlayer2.SetActive(true);
                        newRecordTextPlayer2.text =  NewRecordMessage + player2.score;
                    }
                }
                else
                {
                    if (player1.score > player2.score && player1.score > JsonManager.GetHighScore())
                    {
                        newRecordPanelPlayer1.SetActive(true);
                        newRecordTextPlayer1.text =  NewRecordMessage + player1.score;
                    }
                    else if (player2.score > JsonManager.GetHighScore())
                    {
                        newRecordPanelPlayer2.SetActive(true);
                        newRecordTextPlayer2.text = NewRecordMessage + player2.score;
                    }
                }

                scoreTextPlayer1.text = "Score: " + player1.score;
                scoreTextPlayer2.text = "Score: " + player2.score;
                
                timerText.gameObject.transform.parent.gameObject.SetActive(false);
                
                gameOverPanelScrollRectPlayer1.scrollSensitivity = gameOverPanelContainerPlayer1.transform.childCount / 4f;
                gameOverPanelScrollRectPlayer2.scrollSensitivity = gameOverPanelContainerPlayer2.transform.childCount / 4f;
                
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
                    questionManager.ResetQuestionUI();
                    FindObjectOfType<AudioManager>().PauseSound(ThemeToPlay);
                    FindObjectOfType<AudioManager>().PlaySound("changeController");
                }
                else
                {
                    questionManager.SetQuestionUI();
                    FindObjectOfType<AudioManager>().PlaySound(ThemeToPlay);
                }
                timerText.color = Color.gray;
            }
            else if (timer < 10.49f && !isChanging && !timerText.color.Equals(Color.red))
            {
                FindObjectOfType<AudioManager>().PlaySound("timer");
                timerText.color = Color.red;
            }

            timerText.text = timer.ToString("00");

            //Start Game
            if (!isGameStarted)
            {
                countdownTime -= Time.unscaledDeltaTime;
                countdownTextPlayer1.text = countdownTime.ToString("0");
                countdownTextPlayer2.text = countdownTime.ToString("0");

                if (countdownTime <= 0)
                {
                    countdownTextPlayer1.gameObject.SetActive(false);
                    countdownTextPlayer2.gameObject.SetActive(false);
                    isGameStarted = true;
                }
            }
            DisplayCorrectOrWrongAnswer();
        }

        // This method will make the screen display a checkmark or a cross
        // depending on which player has answered the right/wrong answers.
        private void DisplayCorrectOrWrongAnswer()
        {
            if (!isFlashing) return;
            // Increase or decrease fade
            if (isFadingIn)
            {
                currentFadingTime += Time.unscaledDeltaTime;
            }
            else
            {
                currentFadingTime -= Time.unscaledDeltaTime;
            }

            float alphaValue = (float)(currentFadingTime / secondsFading);
            if (isCorrectAnswerPlayer1)
            {
                checkmarkPlayer1.color = new Color(255, 255, 255, alphaValue);
                crossPlayer1.color = new Color(255, 255, 255, 0);
            }
            else
            {
                crossPlayer1.color = new Color(255, 255, 255, alphaValue);
            }

            if (isCorrectAnswerPlayer2)
            {
                checkmarkPlayer2.color = new Color(255, 255, 255, alphaValue);
                crossPlayer2.color = new Color(255, 255, 255, 0);
            }
            else
            {
                crossPlayer2.color = new Color(255, 255, 255, alphaValue);
            }
            
            // check if fading has reached max
            if (currentFadingTime >= secondsFading)
            {
                isFadingIn = false;
            }

            if (!(currentFadingTime <= 0)) return;
            currentFadingTime = 0;
            isFlashing = false;
            isFadingIn = true;
                
            // make checkmark transparent
            checkmarkPlayer1.color = new Color(255, 255, 255, 0);
            checkmarkPlayer2.color = new Color(255, 255, 255, 0);
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
            if (playerName == "Player1")
            {
                isFlashing = true;
                isCorrectAnswerPlayer1 = true;
                FindObjectOfType<AudioManager>().PlaySound(CorrectAnswerSoundTitle);
            }
            else
            {
                isFlashing = true;
                isCorrectAnswerPlayer2 = true;
                FindObjectOfType<AudioManager>().PlaySound(CorrectAnswerSoundTitle);
            } 
            Player player = playerName == "Player1" ? player1 : player2;
            double totalBuff = (player.buff - player.debuff) / 10f + 1;
            if (totalBuff < 0.5) totalBuff = 0.5;
            player.score += (int)(defaultCorrectQuestionBonus * totalBuff + 0.5);
        } 
        public void WrongQuestion(string playerName)
        {
            if (playerName == "Player1")
            {
                isFlashing = true;
                isCorrectAnswerPlayer1 = false;
                FindObjectOfType<AudioManager>().PlaySound(WrongAnswerSoundTitle);
            }
            else
            {
                isFlashing = true;
                isCorrectAnswerPlayer2 = false;
                FindObjectOfType<AudioManager>().PlaySound(WrongAnswerSoundTitle);
            } 
                 
        }

        public void ScrollGameOverContainer(bool isPlayer1, bool isUp)
        {
            if (isPlayer1)
            {
                if ((gameOverPanelScrollbarPlayer1.value < 0.01 && !isUp) || (gameOverPanelScrollbarPlayer1.value > 0.99 && isUp)) return;
                float value = 1f / (gameOverPanelContainerPlayer1.transform.childCount / 2f);

                float newValue = isUp
                    ? gameOverPanelScrollbarPlayer1.value + value
                    : gameOverPanelScrollbarPlayer1.value - value;

                gameOverPanelScrollbarPlayer1.value = newValue < 0 ? 0 : newValue > 1 ? 1 : newValue;
            }
            else
            {
                if ((gameOverPanelScrollbarPlayer2.value < 0.01 && !isUp) || (gameOverPanelScrollbarPlayer2.value > 0.99 && isUp)) return;
                float value = 1f / (gameOverPanelContainerPlayer2.transform.childCount / 2f);

                float newValue = isUp
                    ? gameOverPanelScrollbarPlayer2.value + value
                    : gameOverPanelScrollbarPlayer2.value - value;

                gameOverPanelScrollbarPlayer2.value = newValue < 0 ? 0 : newValue > 1 ? 1 : newValue;
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public float answerShowTime = 3.5f;
    public PlayerManager playerManagerPlayer;
    
    public GameObject questionBannerPlayer1;
    private TextMeshProUGUI questionPlayer1;
    private TextMeshProUGUI answerAPlayer1;
    private TextMeshProUGUI answerBPlayer1;
    private TextMeshProUGUI answerCPlayer1;
    
    public GameObject questionBannerPlayer2;
    private TextMeshProUGUI questionPlayer2;
    private TextMeshProUGUI answerAPlayer2;
    private TextMeshProUGUI answerBPlayer2;
    private TextMeshProUGUI answerCPlayer2;

    private List<Question> questionsPlayer1;
    private int currentQuestionPlayer1 = 0;
    private float answerTimePlayer1;
    
    private List<Question> questionsPlayer2;
    private int currentQuestionPlayer2 = 0;
    private float answerTimePlayer2;

    private void Awake()
    {
        InitializeUIReferences();
        InitializeQuestions();
    }

    private void InitializeUIReferences()
    {
        questionPlayer1 = questionBannerPlayer1.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        answerAPlayer1 = questionBannerPlayer1.transform.Find("Answer A").GetComponent<TextMeshProUGUI>();
        answerBPlayer1 = questionBannerPlayer1.transform.Find("Answer B").GetComponent<TextMeshProUGUI>();
        answerCPlayer1 = questionBannerPlayer1.transform.Find("Answer C").GetComponent<TextMeshProUGUI>();
        
        questionPlayer2 = questionBannerPlayer2.transform.Find("Question").GetComponent<TextMeshProUGUI>();
        answerAPlayer2 = questionBannerPlayer2.transform.Find("Answer A").GetComponent<TextMeshProUGUI>();
        answerBPlayer2 = questionBannerPlayer2.transform.Find("Answer B").GetComponent<TextMeshProUGUI>();
        answerCPlayer2 = questionBannerPlayer2.transform.Find("Answer C").GetComponent<TextMeshProUGUI>();
    }

    private void InitializeQuestions()
    {
        questionsPlayer1 = new List<Question>()
        {
            new Question("Welke kleur heeft een appel?", "Rood", "Geel", "bruin", 'A'),
            new Question("Welke kleur heeft een banaan?", "Rood", "Geel", "bruin", 'B'),
            new Question("Welke kleur heeft een kiwi?", "Rood", "Geel", "bruin", 'C'),
            new Question("Welke kleur heeft een aardbei?", "Rood", "Geel", "Groen", 'A'),
            // Add more questions here...
        };
        
        questionsPlayer2 = new List<Question>()
        {
            new Question("Welke kleur heeft een peer?", "Groen", "Oranje", "Wit", 'A'),
            new Question("Welke kleur heeft een sinasappel?", "Groen", "Oranje", "Wit", 'B'),
            new Question("Welke kleur heeft een witte druif?", "Groen", "Oranje", "Wit", 'C'),
            new Question("Welke kleur heeft een watermeloen?", "Groen", "Oranje", "Wit", 'A'),
            // Add more questions here...
        };
        
        SetQuestionUI(questionsPlayer1[currentQuestionPlayer1], questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        SetQuestionUI(questionsPlayer2[currentQuestionPlayer2], questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }

    private void SetQuestionUI(Question question, TextMeshProUGUI questionText, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        questionText.text = question.question;
        answerAText.text = question.answerA;
        answerBText.text = question.answerB;
        answerCText.text = question.answerC;
    }

    private void Update()
    {
        UpdateAnswerTimeAndDisplay(1, ref answerTimePlayer1, questionsPlayer1, currentQuestionPlayer1, questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        UpdateAnswerTimeAndDisplay(2, ref answerTimePlayer2, questionsPlayer2, currentQuestionPlayer2, questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }

    private void UpdateAnswerTimeAndDisplay(int playerNumber, ref float answerTime, List<Question> questions, int currentQuestionIndex, TextMeshProUGUI questionText, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        if (!(answerTime > 0)) return;
        
        answerTime -= 1 * Time.deltaTime;
        
        if (!(answerTime <= 0)) return;
        
        ResetAnswerColors(answerAText, answerBText, answerCText);
                
        if (playerNumber == 1)
        {
            currentQuestionPlayer1++;
            if (currentQuestionPlayer1 < questions.Count) SetQuestionUI(questions[currentQuestionPlayer1], questionText, answerAText, answerBText, answerCText);
            else PlayerManager.gameOverPlayer1 = true;
        }
        else if (playerNumber == 2)
        {
            currentQuestionPlayer2++;
            if (currentQuestionPlayer2 < questions.Count) SetQuestionUI(questions[currentQuestionPlayer2], questionText, answerAText, answerBText, answerCText);
            else PlayerManager.gameOverPlayer2 = true;
        }
    }

    private void ResetAnswerColors(TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        answerAText.color = Color.white;
        answerBText.color = Color.white;
        answerCText.color = Color.white;
    }

    public void AnswerQuestionPlayer1(int lane) // 0:left, 1:middle, 2:right
    {
        AnswerQuestion(lane, questionsPlayer1, currentQuestionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1, ref answerTimePlayer1, playerManagerPlayer, "Player1");
    }
    
    public void AnswerQuestionPlayer2(int lane) // 0:left, 1:middle, 2:right
    {
        AnswerQuestion(lane, questionsPlayer2, currentQuestionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2, ref answerTimePlayer2, playerManagerPlayer, "Player2");
    }

    private void AnswerQuestion(int lane, List<Question> questions, int currentQuestionIndex, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText, ref float answerTime, PlayerManager playerManager, string playerName)
    {
        char[] answerOptions = { 'A', 'B', 'C' };
        var selectedAnswer = answerOptions[lane];

        var question = questions[currentQuestionIndex];
        var correctAnswer = selectedAnswer == question.correctAnswer;

        SetAnswerColor(correctAnswer ? Color.green : Color.red, selectedAnswer, answerAText, answerBText, answerCText);

        if (correctAnswer) playerManager.AddPoint(playerName, 25);

        answerTime = answerShowTime;
    }

    private void SetAnswerColor(Color color, char selectedAnswer, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        switch (selectedAnswer)
        {
            case 'A':
                answerAText.color = color;
                break;
            case 'B':
                answerBText.color = color;
                break;
            case 'C':
                answerCText.color = color;
                break;
        }
    }
}

internal class Question
{
    internal readonly string question;
    internal readonly string answerA;
    internal readonly string answerB;
    internal readonly string answerC;
    internal readonly char correctAnswer;

    protected internal Question(string question, string answerA, string answerB, string answerC, char correctAnswer)
    {
        this.question = question;
        this.answerA = answerA;
        this.answerB = answerB;
        this.answerC = answerC;
        this.correctAnswer = correctAnswer;
    }
}

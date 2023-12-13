using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestionManager : MonoBehaviour
{
    public float answerShowTime = 3.5f;
    public PlayerManager playerManager;
    
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

    private List<JSONManager.Question> questionsPlayer1 = new List<JSONManager.Question>();
    private List<string> answersPlayer1 = new();
    private readonly List<JSONManager.Question> wrongQuestionsPlayer1 = new();
    private int currentQuestionPlayer1;
    private float answerTimePlayer1;
    
    private List<JSONManager.Question> questionsPlayer2 = new List<JSONManager.Question>();
    private List<string> answersPlayer2 = new();
    private readonly List<JSONManager.Question> wrongQuestionsPlayer2 = new();
    private int currentQuestionPlayer2;
    private float answerTimePlayer2;

    public GameObject questionPrefab;
    public GameObject questionContainerPlayer1;
    public GameObject questionContainerPlayer2;

    private void Awake()
    {
        InitializeUIReferences();
        StartCoroutine(WaitUntilInitializeQuestions());
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
    
    private bool IsInitialized() { return JSONManager.playerQuestions.questions != null; }
    private bool IsGameStarted() { return PlayerManager.isGameStarted && !PlayerManager.isGamePaused && !PlayerManager.gameOver; }
    
    private IEnumerator WaitUntilInitializeQuestions()
    {
        yield return new WaitUntil(IsInitialized);

        var questions = JSONManager.playerQuestions.questions.ToList();
        
        for (var i = questions.Count; i > 0; i--)
        {
            var random = Random.Range(0, questions.Count);
            var question = questions[random];
            
            if (i % 2 == 0) questionsPlayer1.Add(question);
            else questionsPlayer2.Add(question);
            
            questions.Remove(question);
        }
        
        BlankQuestionUI();
        StartCoroutine(WaitUntilGameStarted());
    }
    
    private IEnumerator WaitUntilGameStarted()
    {
        yield return new WaitUntil(IsGameStarted);

        SetQuestionUI(questionsPlayer1[currentQuestionPlayer1], questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        SetQuestionUI(questionsPlayer2[currentQuestionPlayer2], questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }

    private void SetQuestionUI(JSONManager.Question question, TextMeshProUGUI questionText, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        questionText.text = question.question;
        answerAText.text = question.answerA;
        answerBText.text = question.answerB;
        answerCText.text = question.answerC;
    }
    
    public void ResetQuestionUI()
    {
        SetQuestionUI(questionsPlayer1[currentQuestionPlayer1], questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        SetQuestionUI(questionsPlayer2[currentQuestionPlayer2], questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }
    
    public void BlankQuestionUI()
    {
        SetQuestionUI(new JSONManager.Question(), questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        SetQuestionUI(new JSONManager.Question(), questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }

    private void Update()
    {
        UpdateAnswerTimeAndDisplay(1, ref answerTimePlayer1, questionsPlayer1, questionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1);
        UpdateAnswerTimeAndDisplay(2, ref answerTimePlayer2, questionsPlayer2, questionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2);
    }

    private void UpdateAnswerTimeAndDisplay(int playerNumber, ref float answerTime, List<JSONManager.Question> questions, TextMeshProUGUI questionText, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        if (answerTime <= 0) return;
        
        answerTime -= 1 * Time.deltaTime;
        
        if (answerTime > 0) return;
        
        ResetAnswerColors(answerAText, answerBText, answerCText);
                
        switch (playerNumber)
        {
            case 1:
            {
                currentQuestionPlayer1++;
                if (currentQuestionPlayer1 == questions.Count) AddWrongQuestions();
                if (currentQuestionPlayer1 < questions.Count) SetQuestionUI(questions[currentQuestionPlayer1], questionText, answerAText, answerBText, answerCText);
                else PlayerManager.gameOver = true;
                break;
            }
            case 2:
            {
                currentQuestionPlayer2++;
                if (currentQuestionPlayer2 == questions.Count) AddWrongQuestions();
                if (currentQuestionPlayer2 < questions.Count) SetQuestionUI(questions[currentQuestionPlayer2], questionText, answerAText, answerBText, answerCText);
                else PlayerManager.gameOver = true;
                break;
            }
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
        AnswerQuestion(lane, questionsPlayer1, currentQuestionPlayer1, answerAPlayer1, answerBPlayer1, answerCPlayer1, ref answerTimePlayer1, "Player1");
    }
    
    public void AnswerQuestionPlayer2(int lane) // 0:left, 1:middle, 2:right
    {
        AnswerQuestion(lane, questionsPlayer2, currentQuestionPlayer2, answerAPlayer2, answerBPlayer2, answerCPlayer2, ref answerTimePlayer2, "Player2");
    }

    private void AnswerQuestion(int lane, List<JSONManager.Question> questions, int currentQuestionIndex, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText, ref float answerTime, string playerName)
    {
        string[] answerOptions = { "A", "B", "C" };
        var selectedAnswer = answerOptions[lane];

        var question = questions[currentQuestionIndex];
        var correctAnswer = selectedAnswer == question.correctAnswer;
        
        var questionItem = playerName == "Player1" ? Instantiate(questionPrefab, questionContainerPlayer1.transform) : Instantiate(questionPrefab, questionContainerPlayer2.transform);
        
        questionItem.transform.Find("Question").GetComponent<TextMeshProUGUI>().text = question.question;
                
        var answer = questionItem.transform.Find("Answer").GetComponent<TextMeshProUGUI>();
        answer.text = selectedAnswer;
        answer.color = correctAnswer ? Color.green : Color.red;
                
        questionItem.transform.Find("Correct answer").GetComponent<TextMeshProUGUI>().text = question.correctAnswer.ToString();
        
        switch (playerName)
        {
            case "Player1":
                answersPlayer1.Add(selectedAnswer);
                if (!correctAnswer) wrongQuestionsPlayer1.Add(question);
                break;
            case "Player2":
                answersPlayer2.Add(selectedAnswer);
                if (!correctAnswer) wrongQuestionsPlayer2.Add(question);
                break;
        }

        SetAnswerColor(correctAnswer ? Color.green : Color.red, selectedAnswer, answerAText, answerBText, answerCText);

        if (correctAnswer) playerManager.CorrectQuestion(playerName);

        answerTime = answerShowTime;
    }

    private void SetAnswerColor(Color color, string selectedAnswer, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText)
    {
        switch (selectedAnswer)
        {
            case "A":
                answerAText.color = color;
                break;
            case "B":
                answerBText.color = color;
                break;
            case "C":
                answerCText.color = color;
                break;
        }
    }
    
    private void AddWrongQuestions()
    {
        var lowestWrongQuestionCount = wrongQuestionsPlayer1.Count < wrongQuestionsPlayer2.Count
            ? wrongQuestionsPlayer1.Count
            : wrongQuestionsPlayer2.Count;

        for (var i = 0; i < lowestWrongQuestionCount; i++)
        {
            // Player1
            var randomQuestionPlayer1 = wrongQuestionsPlayer1[Random.Range(0, wrongQuestionsPlayer1.Count)];
            questionsPlayer1.Add(randomQuestionPlayer1);
            wrongQuestionsPlayer1.Remove(randomQuestionPlayer1);
            
            // Player2
            var randomQuestionPlayer2 = wrongQuestionsPlayer2[Random.Range(0, wrongQuestionsPlayer2.Count)];
            questionsPlayer2.Add(randomQuestionPlayer2);
            wrongQuestionsPlayer2.Remove(randomQuestionPlayer2);
        }
    }
}

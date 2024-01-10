using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    private List<JsonManager.Question> questionsPlayer1 = new List<JsonManager.Question>();
    private List<string> answersPlayer1 = new();
    private readonly List<JsonManager.Question> wrongQuestionsPlayer1 = new();
    private int currentQuestionIndex;
    private float answerTime;
    
    private List<JsonManager.Question> questionsPlayer2 = new List<JsonManager.Question>();
    private List<string> answersPlayer2 = new();
    private readonly List<JsonManager.Question> wrongQuestionsPlayer2 = new();

    public GameObject questionPrefab;
    public GameObject questionContainerPlayer1;
    public GameObject questionContainerPlayer2;

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
    
    private bool IsGameStarted() { return PlayerManager.isGameStarted && !PlayerManager.isGamePaused && !PlayerManager.gameOver; }
    
    private void InitializeQuestions()
    {
        var questions = JsonManager.GetQuestions();
        
        for (var i = questions.Count; i > 0; i--)
        {
            var random = Random.Range(0, questions.Count);
            var question = questions[random];
            
            if (i % 2 == 0) questionsPlayer1.Add(question);
            else questionsPlayer2.Add(question);
            
            questions.Remove(question);
        }
        
        ResetQuestionUI();
        StartCoroutine(WaitUntilGameStarted());
    }
    
    private IEnumerator WaitUntilGameStarted()
    {
        yield return new WaitUntil(IsGameStarted);

        SetQuestionUI();
    }

    public void ResetQuestionUI()
    {
        questionPlayer1.text = "";
        answerAPlayer1.text = "";
        answerBPlayer1.text = "";
        answerCPlayer1.text = "";
        
        questionPlayer2.text = "";
        answerAPlayer2.text = "";
        answerBPlayer2.text = "";
        answerCPlayer2.text = "";
    }
    
    public void SetQuestionUI()
    {
        var questionObjectPlayer1 = questionsPlayer1[currentQuestionIndex];
        var questionObjectPlayer2 = questionsPlayer2[currentQuestionIndex];
        
        questionPlayer1.text = questionObjectPlayer1.question;
        answerAPlayer1.text = questionObjectPlayer1.answerA;
        answerBPlayer1.text = questionObjectPlayer1.answerB;
        answerCPlayer1.text = questionObjectPlayer1.answerC;
        
        questionPlayer2.text = questionObjectPlayer2.question;
        answerAPlayer2.text = questionObjectPlayer2.answerA;
        answerBPlayer2.text = questionObjectPlayer2.answerB;
        answerCPlayer2.text = questionObjectPlayer2.answerC;
    }

    private void Update()
    {
        if (answerTime <= 0) return;
        
        answerTime -= 1 * Time.deltaTime;

        if (answerTime > 0 && !((currentQuestionIndex + 1 == questionsPlayer1.Count || currentQuestionIndex + 1 == questionsPlayer2.Count) 
                                && (wrongQuestionsPlayer1.Count == 0 || wrongQuestionsPlayer2.Count == 0))) return;
        
        ResetAnswerColors();

        currentQuestionIndex++;

        if (currentQuestionIndex == questionsPlayer1.Count || currentQuestionIndex == questionsPlayer2.Count) AddWrongQuestions();

        if (currentQuestionIndex < questionsPlayer1.Count && currentQuestionIndex < questionsPlayer2.Count) SetQuestionUI();
        else PlayerManager.gameOver = true;
    }

    private void ResetAnswerColors()
    {
        answerAPlayer1.color = Color.white;
        answerBPlayer1.color = Color.white;
        answerCPlayer1.color = Color.white;
        
        answerAPlayer2.color = Color.white;
        answerBPlayer2.color = Color.white;
        answerCPlayer2.color = Color.white;
    }

    public void AnswerQuestionPlayer1(int lane) // 0:left, 1:middle, 2:right
    {
        AnswerQuestion(lane, questionsPlayer1, currentQuestionIndex, answerAPlayer1, answerBPlayer1, answerCPlayer1, "Player1");
    }
    
    public void AnswerQuestionPlayer2(int lane) // 0:left, 1:middle, 2:right
    {
        AnswerQuestion(lane, questionsPlayer2, currentQuestionIndex, answerAPlayer2, answerBPlayer2, answerCPlayer2, "Player2");
    }

    private void AnswerQuestion(int lane, List<JsonManager.Question> questions, int currentQuestionIndex, TextMeshProUGUI answerAText, TextMeshProUGUI answerBText, TextMeshProUGUI answerCText, string playerName)
    {
        string[] answerOptions = { "A", "B", "C" };
        var selectedAnswer = answerOptions[lane];

        var question = questions[currentQuestionIndex];
        var correctAnswer = selectedAnswer == question.correctAnswer;
        
        var questionItem = playerName == "Player1" ? Instantiate(questionPrefab, questionContainerPlayer1.transform) : Instantiate(questionPrefab, questionContainerPlayer2.transform);
        
        questionItem.transform.Find("Question").GetComponent<TextMeshProUGUI>().text = question.question;
                
        var answers = questionItem.transform.Find("Answers").transform;
        answers.Find("Answer A").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerA;
        answers.Find("Answer B").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerB;
        answers.Find("Answer C").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerC;
        answers.Find("Answer " + selectedAnswer).GetComponent<Image>().color = JsonManager.GetTeamColor(playerName == "Player1" ? 1 : 2);

        questionItem.transform.Find("Labels").transform.Find(question.correctAnswer).GetComponent<TextMeshProUGUI>().color = Color.green;
        
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

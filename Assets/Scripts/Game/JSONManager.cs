using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

public class JSONManager : MonoBehaviour
{
    public TextAsset questionsJson;
    public TextAsset scoresJson;
    
    [Serializable]
    public class Question
    {
        public string question;
        public string answerA;
        public string answerB;
        public string answerC;
        public string correctAnswer;
    }

    [Serializable]
    public class PlayerQuestions { public Question[] questions; }
    
    [Serializable]
    public class Scores
    {
        public List<int> scoresPlayer1;
        public List<int> scoresPlayer2;
    }

    private static PlayerQuestions playerQuestions = new PlayerQuestions();
    private static Scores scores = new Scores();

    private void Start()
    {
        playerQuestions = JsonUtility.FromJson<PlayerQuestions>(questionsJson.text);
        scores = JsonUtility.FromJson<Scores>(scoresJson.text);
    }

    public static List<Question> GetQuestions()
    {
        return playerQuestions.questions.ToList();
    }

    public static void EditQuestion(Question question, string newQuestion)
    {
        if (string.IsNullOrEmpty(newQuestion)) throw new ArgumentException();

        question.question = newQuestion;
        OutputJSONQuestions();
    }
    
    public static void EditAnswerA(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerA = newAnswer;
        OutputJSONQuestions();
    }
    
    public static void EditAnswerB(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerB = newAnswer;
        OutputJSONQuestions();
    }
    
    public static void EditAnswerC(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerC = newAnswer;
        OutputJSONQuestions();
    }

    public static void EditCorrectAnswer(Question question, string newCorrectAnswer)
    {
        if (newCorrectAnswer != "A" && newCorrectAnswer != "B" && newCorrectAnswer != "C") throw new ArgumentOutOfRangeException();

        question.correctAnswer = newCorrectAnswer;
        OutputJSONQuestions();
    }

    public static void AddScores(int scorePlayer1, int scorePlayer2)
    {
        scores.scoresPlayer1.Add(scorePlayer1);
        scores.scoresPlayer2.Add(scorePlayer2);
        OutputJSONScores();
    }

    public static int GetHighscore()
    {
        var highscorePlayer1 = scores.scoresPlayer1.Max();
        var highscorePlayer2 = scores.scoresPlayer2.Max();
        return highscorePlayer1 > highscorePlayer2 ? highscorePlayer1 : highscorePlayer2;
    }
    
    private static void OutputJSONQuestions()
    {
        var strOutput = JsonUtility.ToJson(playerQuestions, true);
        
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Jsons/Questions.json", strOutput);
    }
    
    private static void OutputJSONScores()
    {
        var strOutput = JsonUtility.ToJson(scores, true);
        
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Jsons/Scores.json", strOutput);
    }
}
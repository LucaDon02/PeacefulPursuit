using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

public class JsonManager : MonoBehaviour
{
    private static readonly string MainDataPath = Application.dataPath + "/Resources/Jsons/";
    private static readonly string QuestionsDataPath = MainDataPath + "Questions.json";
    private static readonly string ScoresDataPath = MainDataPath + "Scores.json";
    private static readonly string GameDataDataPath = MainDataPath + "GameData.json";
    
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
    public class Questions { public Question[] questions; }
    
    [Serializable]
    public class Scores
    {
        public List<int> scoresPlayer1;
        public List<int> scoresPlayer2;
    }
    
    [Serializable]
    public class GameData
    {
        public int selectedCharacterPlayer1;
        public int selectedCharacterPlayer2;
    }

    private static Questions _questions = new Questions();
    private static Scores _scores = new Scores();
    private static GameData _gameData = new GameData();

    private void Awake()
    {
        _questions = JsonUtility.FromJson<Questions>(System.IO.File.ReadAllText(QuestionsDataPath));
        _scores = JsonUtility.FromJson<Scores>(System.IO.File.ReadAllText(ScoresDataPath));
        _gameData = JsonUtility.FromJson<GameData>(System.IO.File.ReadAllText(GameDataDataPath));
    }

    public static List<Question> GetQuestions()
    {
        return _questions.questions.ToList();
    }

    public static void EditQuestion(Question question, string newQuestion)
    {
        if (string.IsNullOrEmpty(newQuestion)) throw new ArgumentException();

        question.question = newQuestion;
        OutputJsonQuestions();
    }
    
    public static void EditAnswerA(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerA = newAnswer;
        OutputJsonQuestions();
    }
    
    public static void EditAnswerB(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerB = newAnswer;
        OutputJsonQuestions();
    }
    
    public static void EditAnswerC(Question question, string newAnswer)
    {
        if (string.IsNullOrEmpty(newAnswer)) throw new ArgumentException();

        question.answerC = newAnswer;
        OutputJsonQuestions();
    }

    public static void EditCorrectAnswer(Question question, string newCorrectAnswer)
    {
        if (newCorrectAnswer != "A" && newCorrectAnswer != "B" && newCorrectAnswer != "C") throw new ArgumentOutOfRangeException();

        question.correctAnswer = newCorrectAnswer;
        OutputJsonQuestions();
    }

    public static void AddScores(int scorePlayer1, int scorePlayer2)
    {
        _scores.scoresPlayer1.Add(scorePlayer1);
        _scores.scoresPlayer2.Add(scorePlayer2);
        OutputJsonScores();
    }

    public static int GetHighScore()
    {
        var highScorePlayer1 = _scores.scoresPlayer1.Any() ? _scores.scoresPlayer1.Max() : 0;
        var highScorePlayer2 = _scores.scoresPlayer2.Any() ? _scores.scoresPlayer2.Max() : 0;
        return Math.Max(highScorePlayer1, highScorePlayer2);
    }
    
    public static int GetSelectedCharacterPlayer1() { return _gameData.selectedCharacterPlayer1; }
    
    public static int GetSelectedCharacterPlayer2() { return _gameData.selectedCharacterPlayer2; }
    
    public static void SetSelectedCharacterPlayer1(int index) {
        _gameData.selectedCharacterPlayer1 = index;
        OutputJsonGameData();
    }
    
    public static void SetSelectedCharacterPlayer2(int index) {
        _gameData.selectedCharacterPlayer2 = index;
        OutputJsonGameData();
    }
    
    private static void OutputJsonQuestions()
    {
        var strOutput = JsonUtility.ToJson(_questions, true);
        
        System.IO.File.WriteAllText(QuestionsDataPath, strOutput);
    }
    
    private static void OutputJsonScores()
    {
        var strOutput = JsonUtility.ToJson(_scores, true);
        
        System.IO.File.WriteAllText(ScoresDataPath, strOutput);
    }
    
    private static void OutputJsonGameData()
    {
        var strOutput = JsonUtility.ToJson(_gameData, true);
        
        System.IO.File.WriteAllText(GameDataDataPath, strOutput);
    }
}
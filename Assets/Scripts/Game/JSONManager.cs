using System;
using System.Collections.Generic;
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
        public int highscore;
        public List<int> scoresPlayer1;
        public List<int> scoresPlayer2;
    }

    public static PlayerQuestions playerQuestions = new PlayerQuestions();
    public static Scores scores = new Scores();

    private void Start()
    {
        playerQuestions = JsonUtility.FromJson<PlayerQuestions>(questionsJson.text);
        scores = JsonUtility.FromJson<Scores>(scoresJson.text);
    }

    public static void OutputJSONQuestions()
    {
        var strOutput = JsonUtility.ToJson(playerQuestions, true);
        
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Jsons/Questions.json", strOutput);
    }
    
    public static void OutputJSONScores()
    {
        var strOutput = JsonUtility.ToJson(scores, true);
        
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Jsons/Scores.json", strOutput);
    }
}
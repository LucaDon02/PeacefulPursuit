using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

public class JSONManager : MonoBehaviour
{
    public TextAsset questionsJson;
    
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

    public static PlayerQuestions playerQuestions = new PlayerQuestions();

    private void Start() { playerQuestions = JsonUtility.FromJson<PlayerQuestions>(questionsJson.text); }

    public static void OutputJSON()
    {
        var strOutput = JsonUtility.ToJson(playerQuestions, true);
        
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/Jsons/Questions.json", strOutput);
    }
}
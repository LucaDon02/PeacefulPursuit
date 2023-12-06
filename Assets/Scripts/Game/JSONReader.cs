using System;
using UnityEngine;
using UnityEngine.Serialization;

public class JSONReader : MonoBehaviour
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
}
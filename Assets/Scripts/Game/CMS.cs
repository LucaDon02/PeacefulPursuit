using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CMS : MonoBehaviour
{
    private GameObject container;
    private GameObject scrollbar;
    private List<JSONReader.Question> questions;
    public GameObject questionPrefab;

    private void Awake()
    {
        container = transform.GetChild(0).gameObject;
        scrollbar = transform.GetChild(1).gameObject;
        StartCoroutine(WaitUntilInitializeQuestions());
    }
    
    private bool IsInitialized() { return JSONReader.playerQuestions.questions != null; }

    private IEnumerator WaitUntilInitializeQuestions()
    {
        yield return new WaitUntil(IsInitialized);
        
        questions = JSONReader.playerQuestions.questions.ToList();
        foreach (var question in questions)
        {
            var questionItem = Instantiate(questionPrefab, container.transform);
            
            questionItem.transform.Find("Question").GetComponent<TextMeshProUGUI>().text = question.question;
            questionItem.transform.Find("Answer A").GetComponent<TextMeshProUGUI>().text = question.answerA;
            questionItem.transform.Find("Answer B").GetComponent<TextMeshProUGUI>().text = question.answerB;
            questionItem.transform.Find("Answer C").GetComponent<TextMeshProUGUI>().text = question.answerC;
            
            var toggle = question.correctAnswer switch
            {
                "A" => questionItem.transform.Find("Answer A Toggle").GetComponent<Toggle>(),
                "B" => questionItem.transform.Find("Answer B Toggle").GetComponent<Toggle>(),
                "C" => questionItem.transform.Find("Answer C Toggle").GetComponent<Toggle>(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            toggle.isOn = true;
            toggle.interactable = false;
        }
    }
}

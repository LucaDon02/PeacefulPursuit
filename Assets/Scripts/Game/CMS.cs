using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CMS : MonoBehaviour
{
    public GameObject questionsGameObject;
    private GameObject container;
    private List<JsonManager.Question> questions;
    
    public GameObject questionPrefab;
    
    public GameObject questionModal;
    private TMP_InputField modalInputField;
    private JsonManager.Question modalQuestion;
    private string modalItem;

    private void Awake()
    {
        container = questionsGameObject.transform.GetChild(0).gameObject;
        modalInputField = questionModal.transform.GetChild(0).Find("InputField").GetComponent<TMP_InputField>();
        InitializeQuestions();
    }

    private void Update() {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (questionModal.activeSelf) CloseModal();
        else CloseCMS();
    }

    private void InitializeQuestions()
    {
        questions = JsonManager.GetQuestions();
        foreach (var question in questions)
        {
            var questionItem = Instantiate(questionPrefab, container.transform);

            questionItem.GetComponent<CMSQuestionItem>().cms = this;
            questionItem.GetComponent<CMSQuestionItem>().question = question;
            
            questionItem.transform.Find("Question").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.question;
            questionItem.transform.Find("Answer A").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerA;
            questionItem.transform.Find("Answer B").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerB;
            questionItem.transform.Find("Answer C").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerC;
            
            var toggle = question.correctAnswer switch
            {
                "A" => questionItem.transform.Find("Answer A Toggle").GetComponent<Toggle>(),
                "B" => questionItem.transform.Find("Answer B Toggle").GetComponent<Toggle>(),
                "C" => questionItem.transform.Find("Answer C Toggle").GetComponent<Toggle>(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            toggle.interactable = false;
            toggle.isOn = true;
        }
    }

    private void RefreshList()
    {
        if (container.transform.childCount != questions.Count)
        {
            Debug.LogError("Container count and questions count are not the same. " + container.transform.childCount + " != " + questions.Count);
            return;
        }

        for (var i = 0; i < container.transform.childCount; i++)
        {
            var questionItem = container.transform.GetChild(i);
            var question = questions[i];

            if (questionItem.GetComponent<CMSQuestionItem>().question != question)
            {
                Debug.LogError("QuestionItem hasn't got the same question as the following question order");
                return;
            }
            
            questionItem.transform.Find("Question").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.question;
            questionItem.transform.Find("Answer A").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerA;
            questionItem.transform.Find("Answer B").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerB;
            questionItem.transform.Find("Answer C").GetChild(0).GetComponent<TextMeshProUGUI>().text = question.answerC;
            
            var toggleA = questionItem.transform.Find("Answer A Toggle").GetComponent<Toggle>();
            var toggleB = questionItem.transform.Find("Answer B Toggle").GetComponent<Toggle>();
            var toggleC = questionItem.transform.Find("Answer C Toggle").GetComponent<Toggle>();
            
            switch (question.correctAnswer)
            {
                case "A":
                    toggleA.interactable = false;
                    toggleA.isOn = true;

                    toggleB.isOn = false;
                    toggleB.interactable = true;

                    toggleC.isOn = false;
                    toggleC.interactable = true;
                    break;
                case "B":
                    toggleA.isOn = false;
                    toggleA.interactable = true;

                    toggleB.interactable = false;
                    toggleB.isOn = true;

                    toggleC.isOn = false;
                    toggleC.interactable = true;
                    break;
                case "C":
                    toggleA.isOn = false;
                    toggleA.interactable = true;

                    toggleB.isOn = false;
                    toggleB.interactable = true;

                    toggleC.interactable = false;
                    toggleC.isOn = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal void ChangeCorrectAnswer(GameObject questionItem, JsonManager.Question question, string newCorrectAnswer)
    {
        if (question.correctAnswer == newCorrectAnswer) return;

        var toggleOld = question.correctAnswer switch
        {
            "A" => questionItem.transform.Find("Answer A Toggle").GetComponent<Toggle>(),
            "B" => questionItem.transform.Find("Answer B Toggle").GetComponent<Toggle>(),
            "C" => questionItem.transform.Find("Answer C Toggle").GetComponent<Toggle>(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var toggleNew = newCorrectAnswer switch
        {
            "A" => questionItem.transform.Find("Answer A Toggle").GetComponent<Toggle>(),
            "B" => questionItem.transform.Find("Answer B Toggle").GetComponent<Toggle>(),
            "C" => questionItem.transform.Find("Answer C Toggle").GetComponent<Toggle>(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        toggleOld.isOn = false;
        toggleOld.interactable = true;
        
        toggleNew.interactable = false;
        toggleNew.isOn = true;

        JsonManager.EditCorrectAnswer(question, newCorrectAnswer);
    }

    internal void OpenModal(JsonManager.Question question, string item) //A:Answer A, B:Answer B, C:Answer C, "":Question
    {
        modalQuestion = question;
        modalItem = item;
        
        modalInputField.text = item switch
        {
            "A" => question.answerA,
            "B" => question.answerB,
            "C" => question.answerC,
            "" => question.question,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
        
        questionModal.SetActive(true);
    }

    public void SaveModal()
    {
        switch (modalItem)
        {
            case "A":
                JsonManager.EditAnswerA(modalQuestion, modalInputField.text);
                break;
            case "B":
                JsonManager.EditAnswerB(modalQuestion, modalInputField.text);
                break;
            case "C":
                JsonManager.EditAnswerC(modalQuestion, modalInputField.text);
                break;
            case "":
                JsonManager.EditQuestion(modalQuestion, modalInputField.text);
                break;
        }

        CloseModal();
        RefreshList();
    }

    public void CloseModal()
    {
        questionModal.SetActive(false);
        modalInputField.text = "";
        modalQuestion = null;
        modalItem = null;
    }

    public void CloseCMS() { gameObject.SetActive(false); }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMSQuestionItem : MonoBehaviour
{
    internal CMS cms;
    internal JsonManager.Question question;
    public void SwitchCorrectAnswerA(bool value){ if (value) cms.ChangeCorrectAnswer(gameObject, question, "A"); }
    public void SwitchCorrectAnswerB(bool value){ if (value) cms.ChangeCorrectAnswer(gameObject, question, "B"); }
    public void SwitchCorrectAnswerC(bool value){ if (value) cms.ChangeCorrectAnswer(gameObject, question, "C"); }

    public void EditQuestion(){ cms.OpenModal(question, ""); }
    public void EditAnswerA(){ cms.OpenModal(question, "A"); }
    public void EditAnswerB(){ cms.OpenModal(question, "B"); }
    public void EditAnswerC(){ cms.OpenModal(question, "C"); }
}

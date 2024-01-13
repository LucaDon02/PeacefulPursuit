using UnityEngine;

namespace Game
{
/// <summary>
    /// Represents an item in the Content Management System (CMS) for handling and displaying individual questions in the Unity UI.
/// </summary>
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
}

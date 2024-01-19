using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// Manages the main menu functionality, including character selection, readiness, and game start.
    /// Connects to UI elements for player input and interfaces with the JsonManager for data storage and retrieval.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        private int characterIndexPlayer1;//0:Wheel, 1:Amy, 2:Michelle ...
        private int characterIndexPlayer2;//0:Wheel, 1:Amy, 2:Michelle ...
    
        public GameObject[] charactersPlayer1;
        public GameObject[] charactersPlayer2;

        private bool player1Ready;
        private bool player2Ready;

        public GameObject pressButtonPanelPlayer1;
        public GameObject pressButtonPanelPlayer2;
    
        public GameObject readyButtonPlayer1;
        public GameObject readyButtonPlayer2;
    
        public GameObject unReadyButtonPlayer1;
        public GameObject unReadyButtonPlayer2;
    
        public Button nextCharacterPlayer1;
        public Button nextCharacterPlayer2;
    
        public Button previousCharacterPlayer1;
        public Button previousCharacterPlayer2;

        public TextMeshProUGUI highScoreText;
        public GameObject cms;
    
        public Image readyCheckPlayer1;
        public Image readyCheckPlayer2;

        private void Start()
        {
            Time.timeScale = 1;
            
            foreach (var ch in charactersPlayer1) ch.SetActive(false);
            foreach (var ch in charactersPlayer2) ch.SetActive(false);

            pressButtonPanelPlayer1.SetActive(true);
            pressButtonPanelPlayer2.SetActive(true);
        
            foreach (var playerInput in GameObject.FindGameObjectsWithTag("PlayerInput"))
            {
                var inputHandler = playerInput.GetComponent<PlayerInputHandler>();
                inputHandler.mainMenu = this;
                switch (inputHandler.index)
                {
                    case 0:
                        JoinPlayer(true);
                        break;
                    case 1:
                        JoinPlayer(false);
                        break;
                }
                inputHandler.isGameStarted = false;
            }
        }
    
        private void Update() { highScoreText.text = "High Score\n" + JsonManager.GetHighScore(); }

        public void ChangeNextCharacter(GameObject parentObject)
        {
            switch (parentObject.name)
            {
                case "MainMenu Player1":
                    ChangeCharacterUI(0, false);
                    break;
                case "MainMenu Player2":
                    ChangeCharacterUI(1, false);
                    break;
            }
        }

        public void ChangePreviousCharacter(GameObject parentObject)
        {
            switch (parentObject.name)
            {
                case "MainMenu Player1":
                    ChangeCharacterUI(0, true);
                    break;
                case "MainMenu Player2":
                    ChangeCharacterUI(1, true);
                    break;
            }
        }

        public void Ready(Button button)
        {
            switch (button.transform.parent.name)
            {
                case "MainMenu Player1":
                    readyCheckPlayer1.color = new Color(255, 255, 255, 100);
                    player1Ready = true;
                    unReadyButtonPlayer1.SetActive(true);
                    readyButtonPlayer1.SetActive(false);
                    nextCharacterPlayer1.interactable = false;
                    previousCharacterPlayer1.interactable = false;
                    break;
                case "MainMenu Player2":
                    player2Ready = true;
                    readyCheckPlayer2.color = new Color(255, 255, 255, 100);
                    unReadyButtonPlayer2.SetActive(true);
                    readyButtonPlayer2.SetActive(false);
                    nextCharacterPlayer2.interactable = false;
                    previousCharacterPlayer2.interactable = false;
                    break;
            }
        
            if (player1Ready && player2Ready) SceneManager.LoadScene("Level");
        }
    
        public void UnReady(Button button)
        {
            switch (button.transform.parent.name)
            {
                case "MainMenu Player1":
                    readyCheckPlayer1.color = new Color(255, 255, 255, 0);
                    player1Ready = false;
                    readyButtonPlayer1.SetActive(true);
                    unReadyButtonPlayer1.SetActive(false);
                    nextCharacterPlayer1.interactable = true;
                    previousCharacterPlayer1.interactable = true;
                    break;
                case "MainMenu Player2":
                    player2Ready = false;
                    readyCheckPlayer2.color = new Color(255, 255, 255, 0);
                    readyButtonPlayer2.SetActive(true);
                    unReadyButtonPlayer2.SetActive(false);
                    nextCharacterPlayer2.interactable = true;
                    previousCharacterPlayer2.interactable = true;
                    break;
            }
        }
    
        public void PressReadyButton(bool isPlayer1)
        {
            if (isPlayer1)
            {
                if (player1Ready) UnReady(unReadyButtonPlayer1.GetComponent<Button>());
                else Ready(readyButtonPlayer1.GetComponent<Button>());
            }
            else
            {
                if (player2Ready) UnReady(unReadyButtonPlayer2.GetComponent<Button>());
                else Ready(readyButtonPlayer2.GetComponent<Button>());
            }
        }

        public void OpenCMS()
        {
            cms.SetActive(true);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        public void ChangeCharacterUI(int playerIndex, bool isLeft)
        {
            switch (playerIndex)
            {
                case 0:
                    if (player1Ready) break;
                    if (isLeft)
                    {
                        charactersPlayer1[characterIndexPlayer1].SetActive(false);

                        characterIndexPlayer1--;
                        if (characterIndexPlayer1 == -1) characterIndexPlayer1 = charactersPlayer1.Length - 1;

                        charactersPlayer1[characterIndexPlayer1].SetActive(true);
                        JsonManager.SetSelectedCharacterPlayer1(characterIndexPlayer1);
                    }
                    else
                    {
                        charactersPlayer1[characterIndexPlayer1].SetActive(false);

                        characterIndexPlayer1++;
                        if (characterIndexPlayer1 == charactersPlayer1.Length) characterIndexPlayer1 = 0;

                        charactersPlayer1[characterIndexPlayer1].SetActive(true);
                        JsonManager.SetSelectedCharacterPlayer1(characterIndexPlayer1);
                    }
                    break;
            
                case 1:
                    if (player2Ready) break;
                    if (isLeft)
                    {
                        charactersPlayer2[characterIndexPlayer2].SetActive(false);

                        characterIndexPlayer2--;
                        if (characterIndexPlayer2 == -1) characterIndexPlayer2 = charactersPlayer2.Length - 1;

                        charactersPlayer2[characterIndexPlayer2].SetActive(true);
                        JsonManager.SetSelectedCharacterPlayer2(characterIndexPlayer2);
                    }
                    else
                    {
                        charactersPlayer2[characterIndexPlayer2].SetActive(false);

                        characterIndexPlayer2++;
                        if (characterIndexPlayer2 == charactersPlayer2.Length) characterIndexPlayer2 = 0;

                        charactersPlayer2[characterIndexPlayer2].SetActive(true);
                        JsonManager.SetSelectedCharacterPlayer2(characterIndexPlayer2);
                    }
                    break;
            }
        }

        public void JoinPlayer(bool isPlayer1)
        {
            if (isPlayer1)
            {
                foreach (var ch in charactersPlayer1) ch.SetActive(false);
                characterIndexPlayer1 = JsonManager.GetSelectedCharacterPlayer1();
                charactersPlayer1[characterIndexPlayer1].SetActive(true);
                
                pressButtonPanelPlayer1.SetActive(false);
            }
            else
            {
                foreach (var ch in charactersPlayer2) ch.SetActive(false);
                characterIndexPlayer2 = JsonManager.GetSelectedCharacterPlayer2();
                charactersPlayer2[characterIndexPlayer2].SetActive(true);
                
                pressButtonPanelPlayer2.SetActive(false);
            }
        }

        public void LeavePlayer(bool isPlayer1)
        {
            if (isPlayer1)
            {
                player1Ready = false;
                UnReady(unReadyButtonPlayer1.GetComponent<Button>());
                pressButtonPanelPlayer1.SetActive(true);
                foreach (var ch in charactersPlayer1) ch.SetActive(false);
            }
            else
            {
                player2Ready = false;
                UnReady(unReadyButtonPlayer2.GetComponent<Button>());
                pressButtonPanelPlayer2.SetActive(true);
                foreach (var ch in charactersPlayer2) ch.SetActive(false);
            }
        }
    }
}

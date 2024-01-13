using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [NonSerialized] public PlayerController playerController;
        [NonSerialized] public PlayerManager playerManager;
        [NonSerialized] public MainMenu mainMenu;
        [NonSerialized] public int index;
        [NonSerialized] public bool isGameStarted = false;
        // This field decides if the left stick is postioned on the left or right.
        private const double ControllerThreshold = 0.45;
        private const string MainMenuObjectName = "Main Menu";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            index = GetComponent<PlayerInput>().playerIndex;
            mainMenu = GameObject.Find(MainMenuObjectName)?.GetComponent<MainMenu>();

            if (mainMenu == null)
            {
                Debug.LogError($"Could not find {MainMenuObjectName} or MainMenu component.");
                return;
            }
            DisablePressButtonPanel(); 
        }

        private void DisablePressButtonPanel()
        {
            var pressButtonPanel = GameObject.Find($"MainMenu Player{index + 1}")?.transform.Find("PressButtonPanel")?.gameObject;
            if (pressButtonPanel != null)
            {
                pressButtonPanel.SetActive(false);
            }
            else
            {
                Debug.LogError($"Could not find PressButtonPanel for Player{index + 1}.");
            }
        }

        private void HandleMainMenuMovement(Vector2 movementInput)
        {
            if (movementInput.x < -ControllerThreshold) mainMenu.ChangeCharacterUI(index, true);
            else if (movementInput.x > ControllerThreshold) mainMenu.ChangeCharacterUI(index, false);
        }

        private void HandleGameMovement(Vector2 movementInput)
        {
            if (movementInput.x < -ControllerThreshold) playerController.MoveLeft();
            else if (movementInput.x > ControllerThreshold) playerController.MoveRight();

            if (movementInput.y < -ControllerThreshold) playerController.Slide();
            else if (movementInput.y > ControllerThreshold) playerController.Jump();
        }

        private void HandleGameOverMovement(Vector2 movementInput)
        {
            if (movementInput.y < -ControllerThreshold) playerManager.ScrollGameOverContainer(index == 0, false);
            else if (movementInput.y > ControllerThreshold) playerManager.ScrollGameOverContainer(index == 0, true);
        }

        public void OnMove(CallbackContext callbackContext)
        {
            if (!callbackContext.performed) return;
        
            var movementInput = callbackContext.ReadValue<Vector2>();
        
            if (!isGameStarted)
            {
                HandleMainMenuMovement(movementInput);
            }
            else if (!PlayerManager.gameOver)
            {
                HandleGameMovement(movementInput);
            }
            else
            {
                HandleGameOverMovement(movementInput);
            }
        }

        public void Accept(CallbackContext callbackContext)
        {
            if (!callbackContext.performed || isGameStarted) return;
        
            mainMenu.PressReadyButton(index == 0);
        }

        public void Leave(PlayerInput playerInput)
        {
            mainMenu.LeavePlayer(index == 0);
            Destroy(gameObject);
        }
    }
}
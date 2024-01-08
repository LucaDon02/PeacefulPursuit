using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    [NonSerialized] public PlayerController playerController;
    [NonSerialized] public PlayerManager playerManager;
    [NonSerialized] public MainMenu mainMenu;
    [NonSerialized] public int index;
    [NonSerialized] public bool isGameStarted = false;
    // This field decides if the left stick is postioned on the left or right.
    [NonSerialized] public const double ControllerThreshold = 0.45;
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        index = GetComponent<PlayerInput>().playerIndex;
        mainMenu = GameObject.Find("Main Menu").GetComponent<MainMenu>();
        
        GameObject.Find("MainMenu Player" + (index + 1)).transform.Find("PressButtonPanel").gameObject.SetActive(false);
    }

    public void OnMove(CallbackContext callbackContext)
    {
        if (!callbackContext.performed) return;
        
        var v2 = callbackContext.ReadValue<Vector2>();
        
        if (!isGameStarted)
        {
            if (v2.x < -ControllerThreshold) mainMenu.ChangeCharacterUI(index, true);
            else if (v2.x > ControllerThreshold) mainMenu.ChangeCharacterUI(index, false);
        }
        else if (!PlayerManager.gameOver)
        {
            if (v2.x < -ControllerThreshold) playerController.MoveLeft();
            else if (v2.x > ControllerThreshold) playerController.MoveRight();

            if (v2.y < -ControllerThreshold) playerController.Slide();
            else if (v2.y > ControllerThreshold) playerController.Jump();
        }
        else
        {
            if (v2.y < -ControllerThreshold) playerManager.ScrollGameOverContainer(index == 0, false);
            else if (v2.y > ControllerThreshold) playerManager.ScrollGameOverContainer(index == 0, true);
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
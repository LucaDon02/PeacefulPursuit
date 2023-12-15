using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    [NonSerialized] public PlayerController playerController;
    [NonSerialized] public int index;
    [NonSerialized] public bool isGameStarted = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        index = GetComponent<PlayerInput>().playerIndex;
        GameObject.Find("MainMenu Player" + (index + 1)).transform.Find("PressButtonPanel").gameObject.SetActive(false);
    }

    public void OnMove(CallbackContext callbackContext)
    {
        if (!callbackContext.performed || !isGameStarted) return;
        
        var v2 = callbackContext.ReadValue<Vector2>();

        if (v2.x < -0.45) playerController.MoveLeft();
        else if (v2.x > 0.45) playerController.MoveRight();

        if (v2.y < -0.45) playerController.Slide();
        else if (v2.y > 0.45) playerController.Jump();
    }
}
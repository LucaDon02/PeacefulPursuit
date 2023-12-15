using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerInput>().playerIndex switch
        {
            0 => GameObject.Find("Player1").GetComponent<PlayerController>(),
            1 => GameObject.Find("Player2").GetComponent<PlayerController>(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void OnMove(CallbackContext callbackContext)
    {
        if (!callbackContext.performed) return;
        
        var v2 = callbackContext.ReadValue<Vector2>();

        if (v2.x < -0.45) playerController.MoveLeft();
        else if (v2.x > 0.45) playerController.MoveRight();

        if (v2.y < -0.45) playerController.Slide();
        else if (v2.y > 0.45) playerController.Jump();
    }
}
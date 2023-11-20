﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 move;
    public float forwardSpeed;
    public float maxSpeed;

    public int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2.5f;//The distance between tow lanes

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float gravity = -12f;
    public float jumpHeight = 2;
    private Vector3 velocity;

    public Animator animator;
    private bool isSliding = false;

    public float slideDuration = 1.5f;

    bool toggle = false;

    public bool isPlayer1;
    private KeyCode leftButton;
    private KeyCode rightButton;
    private KeyCode jumpButton;
    private KeyCode slideButton;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
        
        leftButton = isPlayer1 ? KeyCode.A : KeyCode.LeftArrow;
        rightButton = isPlayer1 ? KeyCode.D : KeyCode.RightArrow;
        jumpButton = isPlayer1 ? KeyCode.W : KeyCode.UpArrow;
        slideButton = isPlayer1 ? KeyCode.S : KeyCode.DownArrow;
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver) return;

        //Increase Speed
        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed) forwardSpeed += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f) Time.timeScale += 0.005f * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver) return;

        animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0) velocity.y = -1f;

        if (isGrounded)
        {
            if (Input.GetKeyDown(jumpButton)) Jump();
            if (Input.GetKeyDown(slideButton) && !isSliding) StartCoroutine(Slide());
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            if (Input.GetKeyDown(slideButton) && !isSliding)
            {
                StartCoroutine(Slide());
                velocity.y = -10;
            }                

        }
        controller.Move(velocity * Time.deltaTime);

        //Gather the inputs on which lane we should be
        if (Input.GetKeyDown(rightButton))
        {
            desiredLane++;
            if (desiredLane == 3) desiredLane = 2;
        }
        if (Input.GetKeyDown(leftButton))
        {
            desiredLane--;
            if (desiredLane == -1) desiredLane = 0;
        }

        //Calculate where we should be in the future
        var targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        targetPosition.x += transform.name == "Player1" ? -25 : 25;
        switch (desiredLane)
        {
            case 0:
                targetPosition += Vector3.left * laneDistance;
                break;
            case 2:
                targetPosition += Vector3.right * laneDistance;
                break;
        }
        
        if (transform.position != targetPosition)
        {
            var diff = targetPosition - transform.position;
            var moveDir = diff.normalized * 30 * Time.deltaTime;
            controller.Move(moveDir.sqrMagnitude < diff.magnitude ? moveDir : diff);
        }

        controller.Move(move * Time.deltaTime);
    }

    private void Jump()
    {   
        StopCoroutine(Slide());
        animator.SetBool("isSliding", false);
        animator.SetTrigger("jump");
        controller.center = Vector3.zero;
        controller.height = 2;
        isSliding = false;
   
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Obstacle") return;
        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().PlayerHitObstacle(isPlayer1);
        // PlayerManager.gameOver = true;
        FindObjectOfType<AudioManager>().PlaySound("PlayerDamage");
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(0.25f/ Time.timeScale);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds((slideDuration - 0.25f)/Time.timeScale);

        animator.SetBool("isSliding", false);

        controller.center = Vector3.zero;
        controller.height = 2;

        isSliding = false;
    }
}

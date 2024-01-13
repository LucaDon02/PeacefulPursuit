using System.Collections;
using Game;
using UnityEngine;

namespace Player
{
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
        public bool isSliding;

        public float slideDuration = 1.5f;

        private bool toggle;

        public bool isPlayer1;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            Time.timeScale = 1.2f;
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

        private void Update()
        {
            if (!PlayerManager.isGameStarted || PlayerManager.gameOver) return;

            animator.SetBool("isGameStarted", true);
            move.z = forwardSpeed;

            isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
            animator.SetBool("isGrounded", isGrounded);
        
            switch (isGrounded)
            {
                case true when velocity.y < 0:
                    velocity.y = -1f;
                    break;
                case false:
                    velocity.y += gravity * Time.deltaTime;
                    break;
            }

            controller.Move(velocity * Time.deltaTime);

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
                Vector3 diff = targetPosition - transform.position;
                Vector3 moveDir = diff.normalized * (30 * Time.deltaTime);
                controller.Move(moveDir.sqrMagnitude < diff.magnitude ? moveDir : diff);
            }

            controller.Move(move * Time.deltaTime);
        }

        public void Jump()
        {
            if (!isGrounded) return;
        
            StopCoroutine(SlideCoroutine());
            animator.SetBool("isSliding", false);
            animator.SetTrigger("jump");
            controller.center = Vector3.zero;
            controller.height = 2;
            isSliding = false;
   
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
            FindObjectOfType<AudioManager>().PlaySound("cartoonJump");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag != "Obstacle") return;
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>().PlayerHitObstacle(isPlayer1);
            FindObjectOfType<AudioManager>().PlaySound("PlayerDamage");
        }

        private IEnumerator SlideCoroutine()
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
    
        public void MoveLeft()
        {
            desiredLane--;
            if (desiredLane == -1) desiredLane = 0;
        }

        public void MoveRight()
        {
            desiredLane++;
            if (desiredLane == 3) desiredLane = 2;
        }

        public void Slide()
        {
            if (isSliding) return;
            StartCoroutine(SlideCoroutine());
            if (!isGrounded) velocity.y = -10;
        }
    }
}

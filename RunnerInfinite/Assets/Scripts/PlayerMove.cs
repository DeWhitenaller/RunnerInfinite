using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Touch;

public class PlayerMove : MonoBehaviour
{
    public float speed, jumpStrength, groundDistance, gravity;
    public bool directionCanBeRequested;
    public bool directionChangeFailed;

    [SerializeField] LayerMask groundMask;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] PlayerStateManager stateManager;
    [SerializeField] TileSpawnManager tileSpawnManager;
    [SerializeField] Transform rotationWatcher;
    [SerializeField] GameManager gameManager;


    bool isGrounded;
    Vector3 velocity;
    float transitionTimer = 0f;


    [SerializeField] string nextMove = "unknown";
    [SerializeField] string nextDir = "unknown";
    [SerializeField] float directionChangeSpeed;
    [SerializeField] bool nextMoveEnabled;
    [SerializeField] LeanTouch leanTouch;

    [SerializeField] int switchAmount = -1;

    [Range(-1, 1)]
    public float tilt;

    [SerializeField] float tiltSpeed;


    void Start()
    {
        LeanTouch.OnFingerSwipe += CheckNextMove;
        //StartCoroutine(CheckAccelerometer());
    }

    void Update()
    {
        CheckIfGrounded();
        ApplyPhysics();
        ApplyMovement();

        Tilt();

        tilt = Input.GetAxis("Horizontal");
    }

    IEnumerator CheckAccelerometer()
    {
        yield return new WaitForSeconds(0f);

        tilt = Input.acceleration.x * 2;
        StartCoroutine(CheckAccelerometer());
    }

    void Tilt()
    {
        
        Vector3 dir = transform.right * Time.deltaTime * tiltSpeed * tilt;
        controller.Move(dir);        
    }

    public void ChangeDirection(string correctDirection)
    {
        if (gameManager.isInHyperDrive)
        {
            if(correctDirection == "both")
            {
                int random = Random.Range(0, 2);

                if (random == 0) nextDir = "left";
                else nextDir = "right";
            }
            else
            {
                nextDir = correctDirection;
            }
        }

        if(correctDirection == "left")
        {
            if (nextDir == "left")
            {
                switchAmount--;
                RotatePlayer();
            }
            else
            {
                directionChangeFailed = true;
            }
        }
        else if (correctDirection == "right")
        {
            if (nextDir == "right")
            {
                switchAmount++;
                RotatePlayer();
            }
            else
            {
                directionChangeFailed = true;
            }
        }
        else if (correctDirection == "both")
        {
            if (nextDir == "left")
            {
                switchAmount--;
                RotatePlayer();
            }
            else if (nextDir == "right")
            {
                switchAmount++;
                RotatePlayer();
            }
            else
            {
                directionChangeFailed = true;
            }
        }


        nextDir = "unknown";
        directionCanBeRequested = false;
    }

    void RotatePlayer()
    {
        Quaternion playerHolderRotation = Quaternion.Euler(0, 90 * switchAmount, 0);
        transform.DORotateQuaternion(playerHolderRotation, directionChangeSpeed);
        rotationWatcher.rotation = Quaternion.Euler(0, 90 * switchAmount, 0);
        tileSpawnManager.SpawnTile();
    }


    void ApplyPhysics()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyMovement()
    {
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
    }


    public void ChangeState(PlayerStateManager.State switchTo)
    {
        nextMoveEnabled = false;

        switch (switchTo)
        {
            case PlayerStateManager.State.Running:
                ResizeCollider(false);
                Debug.Log("ToRun");
                transitionTimer = 0f;
                anim.SetBool("isRunning", true);
                break;

            case PlayerStateManager.State.Jumping:
                ResizeCollider(false);
                transitionTimer = 0f;
                anim.SetBool("isRunning", false);
                anim.SetTrigger("Jump");
                break;

            case PlayerStateManager.State.Sliding:
                ResizeCollider(true);
                transitionTimer = 0f;
                anim.SetBool("isRunning", false);
                anim.SetTrigger("Slide");
                break;
        }
    }

    public void RunningState()
    {
        if (gameManager.isInHyperDrive) return;

        nextMoveEnabled = true;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            CheckPlayerJumpInput();
            CheckPlayerSlideInput();
        }
    }

    public void JumpingState()
    {
        if (transitionTimer > 0.2f)
        {
            nextMoveEnabled = true;
        }
        else
        {
            transitionTimer += Time.deltaTime;
        }

        if (isGrounded && transitionTimer > 0.05f)
        {
            stateManager.ChangeState(PlayerStateManager.State.Running);
        }

    }

    public void SlidingState()
    {

        if (transitionTimer > 0.2f)
        {
            nextMoveEnabled = true;
        }
        else
        {
            transitionTimer += Time.deltaTime;
        }


        if (anim.IsInTransition(0))
        {
            stateManager.ChangeState(PlayerStateManager.State.Running);
        }
    }

    void CheckNextMove(LeanFinger finger)
    {
        if (!nextMoveEnabled || gameManager.isInHyperDrive) return;

        if(Mathf.Abs(finger.SwipeScreenDelta.y) > Mathf.Abs(finger.SwipeScreenDelta.x))
        {
            if (finger.SwipeScreenDelta.y > leanTouch.SwipeThreshold)
            {
                nextMove = "jump";
            }
            else if (finger.SwipeScreenDelta.y < -leanTouch.SwipeThreshold)
            {
                nextMove = "slide";
            }
        }
        
        if(directionCanBeRequested && Mathf.Abs(finger.SwipeScreenDelta.y) < Mathf.Abs(finger.SwipeScreenDelta.x))
        {
            if (finger.SwipeScreenDelta.x > leanTouch.SwipeThreshold)
            {
                nextDir = "right";
            }
            else if (finger.SwipeScreenDelta.x < -leanTouch.SwipeThreshold)
            {
                nextDir = "left";
            }
        }
    }

    void ResizeCollider(bool slide)
    {
        if (slide)
        {
            controller.height = 1f;
            controller.center = new Vector3(0, -0.5f, 0);
        }
        else
        {
            controller.height = 2f;
            controller.center = Vector3.zero;
        }
    }


    void CheckPlayerJumpInput()
    {
        if (nextMove == "jump" && isGrounded)
        {
            Jump();
        }
    }

    void CheckPlayerSlideInput()
    {
        if (nextMove == "slide")
        {
            Slide();
        }
    }

    void Jump()
    {
        nextMove = "unknown";
        velocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
        stateManager.ChangeState(PlayerStateManager.State.Jumping);
    }
    void Slide()
    {
        nextMove = "unknown";
        stateManager.ChangeState(PlayerStateManager.State.Sliding);
    }

}


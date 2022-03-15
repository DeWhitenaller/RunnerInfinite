using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Touch;

public class PlayerMoveNoTouch : MonoBehaviour
{
    public float speed, jumpStrength, groundDistance, gravity;
    bool isGrounded;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject cam;
    CharacterController controller;
    Vector3 velocity;
    int offset = 0;
    public Animator anim;

    public PlayerStateManager stateManager;

    float transitionTimer = 0f;


    [SerializeField] string nextMove = "unknown";
    [SerializeField] float dashSpeed;



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckIfGrounded();
        ApplyPhysics();
        ApplyMovement();
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
        switch (switchTo)
        {
            case PlayerStateManager.State.Running:
                transitionTimer = 0f;
                anim.SetBool("isRunning", true);
                break;

            case PlayerStateManager.State.Jumping:
                transitionTimer = 0f;
                anim.SetBool("isRunning", false);
                anim.SetTrigger("Jump");
                break;

            case PlayerStateManager.State.Sliding:
                transitionTimer = 0f;
                anim.SetBool("isRunning", false);
                anim.SetTrigger("Slide");
                break;
        }
    }

    public void RunningState()
    {
        CheckNextMove();
        CheckPlayerMoveInput();

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
            CheckNextMove();
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
            CheckNextMove();
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

    void CheckNextMove()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            nextMove = "Right";
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            nextMove = "Left";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            nextMove = "Jump";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            nextMove = "Slide";
        }
    }

    void CheckPlayerMoveInput()
    {
        if (nextMove == "Right" && offset < 5)
        {
            nextMove = "unknown";
            Right();
        }
        else if (nextMove == "Left" && offset > -5)
        {
            nextMove = "unknown";
            Left();
        }
    }

    void CheckPlayerJumpInput()
    {
        if (nextMove == "Jump" && isGrounded)
        {
            Jump();
        }
    }

    void CheckPlayerSlideInput()
    {
        if (nextMove == "Slide")
        {
            Slide();
        }
    }

    void Left()
    {
        nextMove = "unknown";
        offset -= 3;
        cam.transform.DOLocalRotate(new Vector3(0, 0, 5), dashSpeed);
        transform.DOMoveZ(offset, dashSpeed).OnComplete(() => { cam.transform.DOLocalRotate(new Vector3(0, 0, 0), dashSpeed); }); ;
    }
    void Right()
    {
        nextMove = "unknown";
        offset += 3;
        cam.transform.DOLocalRotate(new Vector3(0, 0, -5), dashSpeed);
        transform.DOMoveZ(offset, dashSpeed).OnComplete(() => { cam.transform.DOLocalRotate(new Vector3(0, 0, 0), dashSpeed); });
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


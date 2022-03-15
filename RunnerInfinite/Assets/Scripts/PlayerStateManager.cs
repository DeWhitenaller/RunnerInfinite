using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum State
    {
        Running,
        Jumping,
        Sliding
    }

    public State currentState;
    public AnimationManager animManager;
    public PlayerMove playerMove;

    void Start()
    {
        ChangeState(State.Running);
    }

    void Update()
    {
        CheckCurrentState();

        animManager.TiltMainCam(playerMove.tilt);
    }

    void CheckCurrentState()
    {
        switch (currentState)
        {
            case State.Running:
                playerMove.RunningState();
                animManager.Run();
                break;

            case State.Jumping:
                playerMove.JumpingState();
                animManager.JumpingState();
                break;

            case State.Sliding:
                playerMove.SlidingState();
                animManager.Slide();
                break;

        }
    }

    public void ChangeState(State switchTo)
    {
        switch (switchTo)
        {
            case State.Running:
                animManager.ChangeState(switchTo);
                playerMove.ChangeState(switchTo);
                break;

            case State.Jumping:
                animManager.ChangeState(switchTo);
                playerMove.ChangeState(switchTo);
                break;

            case State.Sliding:
                animManager.ChangeState(switchTo);
                playerMove.ChangeState(switchTo);
                break;
        }

        currentState = switchTo;
    }
}

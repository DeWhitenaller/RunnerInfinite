using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Transform camHolder;
    [SerializeField] Transform characterNeck;
    [SerializeField] Camera mainCam;
    
    [SerializeField] float slideSmoothness;
    [SerializeField] Vector3 slideRotation;
    
    [SerializeField] float jumpSmoothness;
    [SerializeField] Vector3 jumpRotation;

    float runSmoothness = 2;
    [SerializeField] float transitionToRunSmoothness;
    float tempRunSmoothness;
    public float runOverDrive;
    float runRotationTimer = 0f;

    [SerializeField] float tiltStrength;
    [SerializeField] float tiltSpeed;


    void Start()
    {
        tempRunSmoothness = runSmoothness;
    }

    void Update()
    {

    }


    public void ChangeState(PlayerStateManager.State playerState)
    {
        switch (playerState)
        {
            case PlayerStateManager.State.Running:
                runRotationTimer = 0f;
                break;

            case PlayerStateManager.State.Jumping:
                break;

            case PlayerStateManager.State.Sliding:
                break;
        }
    }


    public void JumpingState()
    {
        Quaternion rotation = Quaternion.Euler(jumpRotation);
        camHolder.localRotation = Quaternion.Lerp(camHolder.localRotation, rotation, Time.deltaTime * jumpSmoothness);
    }


    public void Slide()
    {
        Quaternion rotation = Quaternion.Euler(slideRotation);
        camHolder.localRotation = Quaternion.Lerp(camHolder.localRotation, rotation, Time.deltaTime * slideSmoothness);
    }

    public void Run()
    {
        if (runRotationTimer < transitionToRunSmoothness)
        {
            runRotationTimer += Time.deltaTime;
            runSmoothness = Mathf.Lerp(1, tempRunSmoothness * runOverDrive, runRotationTimer / transitionToRunSmoothness);
        }
        else
        {
            runSmoothness = tempRunSmoothness * runOverDrive;
        }

        camHolder.localRotation = Quaternion.Lerp(camHolder.localRotation, characterNeck.localRotation, Time.deltaTime * runSmoothness);
    }

    public void TiltMainCam(float tilt)
    {
        Quaternion tiltRotation = Quaternion.Euler(0, tilt * tiltStrength, (tilt * -1) * tiltStrength);
        mainCam.transform.localRotation = Quaternion.Lerp(mainCam.transform.localRotation, tiltRotation, Time.deltaTime * tiltSpeed);
    }
}

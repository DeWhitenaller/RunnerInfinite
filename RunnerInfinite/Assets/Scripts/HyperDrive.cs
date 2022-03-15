using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class HyperDrive : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] AnimationManager animManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] Animator charAnimator;
    [SerializeField] VisualEffect hyperDriveVFX;
    [SerializeField] MeshRenderer hyperDriveCone;
    [SerializeField] GameObject vHSEffect;

    Vector3 oldVHSScale;
    float oldGameSpeed, oldFOV, oldRunOverDrive, oldRunAnimSpeed;
    [SerializeField] float hyperDriveSpeed, fOV, runOverDrive, runAnimSpeed;
    [SerializeField] Vector3 vHSScale;
    [SerializeField] float activateIn, activateOut, deactivateIn, deactivateOut;

    public bool hyperDrive;
    public bool deactivateHyperDrive;

    void Start()
    {
        hyperDriveCone.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hyperDrive)
        {
            hyperDrive = false;
            gameManager.isInHyperDrive = true;
            oldFOV = mainCam.fieldOfView;

            DOTween.Sequence()
                .Append(DOTween.To(() => mainCam.fieldOfView, x => mainCam.fieldOfView = x, 65, activateIn)).SetEase(Ease.Flash)
                .Append(DOTween.To(() => mainCam.fieldOfView, x => mainCam.fieldOfView = x, 80, activateOut))
                .OnComplete(() => { ActivateHyperDrive(); });
        }


        if (deactivateHyperDrive)
        {
            deactivateHyperDrive = false;
            DOTween.Sequence()
                .AppendCallback(() => { hyperDriveVFX.Stop(); })
                .Append(DOTween.To(() => mainCam.fieldOfView, x => mainCam.fieldOfView = x, oldFOV - 10, deactivateIn)).SetEase(Ease.InBounce)
                .AppendCallback(() => { DeactivateHyperDrive(); })
                .Append(DOTween.To(() => mainCam.fieldOfView, x => mainCam.fieldOfView = x, oldFOV, deactivateOut));

        }
    }

    public void ActivateHyperDrive()
    {
        oldVHSScale = vHSEffect.transform.localScale;
        oldGameSpeed = gameManager.gameSpeed;
        oldRunAnimSpeed = charAnimator.speed;
        oldRunOverDrive = animManager.runOverDrive;

        DOTween.To(() => mainCam.fieldOfView, x => mainCam.fieldOfView = x, fOV, 0.2f);

        vHSEffect.transform.localScale = vHSScale;
        hyperDriveCone.gameObject.SetActive(true);
        hyperDriveCone.material.SetFloat("Amount", 0.15f);
        hyperDriveVFX.Play();
        gameManager.SetSpeed(hyperDriveSpeed);
        animManager.runOverDrive = runOverDrive;
        charAnimator.speed = runAnimSpeed;
    }

    public void DeactivateHyperDrive()
    {
        vHSEffect.transform.localScale = oldVHSScale;
        hyperDriveCone.material.SetFloat("Amount", 0f);
        hyperDriveCone.gameObject.SetActive(false);
        gameManager.SetSpeed(oldGameSpeed);
        animManager.runOverDrive = oldRunOverDrive;
        charAnimator.speed = oldRunAnimSpeed;
        gameManager.isInHyperDrive = false;
    }
}

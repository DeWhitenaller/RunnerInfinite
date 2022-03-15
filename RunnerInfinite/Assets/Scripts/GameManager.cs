using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    public float gameSpeed = 1f;
    public float startPlayerSpeed;
    [SerializeField] bool speedUp;
    [SerializeField] bool slowDown;
    bool changeColor;
    bool changing;

    [SerializeField] VolumeProfile volumeProfile;
    ColorAdjustments colorAdjustments;
    public List<float> hueShiftColors;
    [SerializeField] float smooth;
    float targetColor;

    public bool isInHyperDrive;


    private void Awake()
    {
        volumeProfile.TryGet(out colorAdjustments);
        SetPlayerSpeed();
    }

    void Update()
    {
        if (speedUp)
        {
            speedUp = false;

            SpeedUp(0.1f);
        }

        if (slowDown)
        {
            slowDown = false;

            SlowDown(0.1f);
        }

        if (changeColor)
        {
            changeColor = false;

            ChangeVolumeHueShift(Random.Range(0, hueShiftColors.Count));
        }
    }


    void SetPlayerSpeed()
    {
        playerMove.speed = startPlayerSpeed * gameSpeed;
    }

    public void SpeedUp(float speedChange)
    {
        gameSpeed += speedChange;
        SetPlayerSpeed();
    }

    public void SlowDown(float speedChange)
    {
        gameSpeed -= speedChange;
        SetPlayerSpeed();
    }

    public void SetSpeed(float speed)
    {
        gameSpeed = speed;
        SetPlayerSpeed();
    }

    public void ChangeVolumeHueShift(int randomInt)
    {
        targetColor = hueShiftColors[randomInt];

        changing = true;

        StartCoroutine(ChangeShift());
    }

    public IEnumerator ChangeShift()
    {
        while (changing)
        {
            float color = colorAdjustments.hueShift.value;

            if (color < targetColor - 0.5f || color > targetColor + 0.5f)
            {
                float current;

                if (targetColor > color)
                {
                    current = Mathf.Lerp(color, targetColor, Time.deltaTime * smooth);
                }
                else
                {
                    current = Mathf.Lerp(color, targetColor, Time.deltaTime * smooth);
                }

                colorAdjustments.hueShift.Override(current);
                yield return null;
            }
            else changing = false;
        }
    }
}

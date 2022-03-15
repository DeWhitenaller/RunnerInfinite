using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI distanceText;
    float distance;
    bool speedUp;


    void Update()
    {
        IncreasePlayerDistance();

        if (gameManager.gameSpeed >= 2.2f) return;

        if (Mathf.RoundToInt(distance) % 100 == 0 && !speedUp)
        {
            IncreaseGameSpeed();
        }
    }

    void IncreasePlayerDistance()
    {
        distance += Time.deltaTime * gameManager.gameSpeed * 10;
        distanceText.text = Mathf.RoundToInt(distance).ToString();
    }

    void IncreaseGameSpeed()
    {
        speedUp = true;
        gameManager.SpeedUp(0.1f);
        gameManager.ChangeVolumeHueShift(Random.Range(0, gameManager.hueShiftColors.Count));
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.1f);

        speedUp = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnManager : MonoBehaviour
{
    [SerializeField] float dissolveDistanceStart, dissolveDistanceEnd;
    Transform player;

    [SerializeField] List<GameObject> obstacleSpawnPoints, obstaclePrefabs;
    [SerializeField] List<MeshRenderer> spawnedObstacles;
    [SerializeField] GameManager gameManager;
    float timer = 0f;
    bool hyperDriveWasActive;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnObstacles();
    }

    void Update()
    {
        if (gameManager.isInHyperDrive)
        {
            DissolveObstacles();
        }
        else
        {
            if (hyperDriveWasActive) return;

            timer = 0f;
            MakeObstaclesVisible();
        }
    }

    void MakeObstaclesVisible()
    {
        foreach (var obstacle in spawnedObstacles)
        {
            if (obstacle.material.GetFloat("CutoffHeight") >= 2)
            {
                continue;
            }
            else
            {
                float currentDistance = Vector3.Distance(player.position, obstacle.transform.position);
                currentDistance -= dissolveDistanceEnd;

                if (currentDistance < dissolveDistanceStart)
                {
                    float y = Mathf.Lerp(2, -1.1f, currentDistance / dissolveDistanceStart);
                    obstacle.material.SetFloat("CutoffHeight", y);
                }
            }
        }
    }

    void DissolveObstacles()
    {
        if (timer == 0f)
        {
            hyperDriveWasActive = true;

            foreach (var obstacle in spawnedObstacles)
            {
                obstacle.GetComponent<BoxCollider>().enabled = false;
            }
        }

        timer += Time.deltaTime / 10;

        foreach (var obstacle in spawnedObstacles)
        {
            if (obstacle.material.GetFloat("CutoffHeight") <= -1.1f) continue;

            float y = Mathf.Lerp(obstacle.material.GetFloat("CutoffHeight"), -1.1f, timer);
            obstacle.material.SetFloat("CutoffHeight", y);
        }
    }

    void SpawnObstacles()
    {

        foreach (var spawnPoint in obstacleSpawnPoints)
        {
            float random = Random.Range(0, 101);
            float spawnObstacle = Random.Range(0, 3);

            if(spawnObstacle < 2)
            {
                if(random < 25)
                {
                    GameObject currentObstacle = Instantiate(obstaclePrefabs[0], spawnPoint.transform, true);
                    currentObstacle.transform.localPosition = new Vector3(0, 0.5f, 0);
                    currentObstacle.transform.localRotation = Quaternion.identity;
                    spawnedObstacles.Add(currentObstacle.GetComponent<MeshRenderer>());
                }
                else if(random < 50)
                {
                    GameObject currentObstacle = Instantiate(obstaclePrefabs[1], spawnPoint.transform, true);
                    currentObstacle.transform.localPosition = new Vector3(0, 13.5f, 0);
                    currentObstacle.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    spawnedObstacles.Add(currentObstacle.GetComponent<MeshRenderer>());
                }
                else
                {
                    GameObject currentObstacle = Instantiate(obstaclePrefabs[2], spawnPoint.transform, true);

                    float randomDir = Random.Range(0, 2);
                    if(randomDir == 0) currentObstacle.transform.position = spawnPoint.transform.position + spawnPoint.transform.right * 6.25f;
                    else currentObstacle.transform.position = spawnPoint.transform.position + spawnPoint.transform.right * -6.25f;

                    currentObstacle.transform.localRotation = Quaternion.identity;
                    spawnedObstacles.Add(currentObstacle.GetComponent<MeshRenderer>());
                }
            }
            else
            {
                continue;
            }

            SpawnCoins();
        }
    }

    void SpawnCoins()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] List<GameObject> walls;

    Transform player;
    public float distanceStart;
    public float distanceEnd;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        MoveWallsUp();
    }

    void MoveWallsUp()
    {
        foreach (var wall in walls)
        {
            if (wall.transform.position.y >= 7)
            {
                continue;
            }
            else
            {
                float currentDistance = Vector3.Distance(player.transform.position, wall.transform.position);
                currentDistance -= distanceEnd;

                if (currentDistance < distanceStart)
                {
                    Vector3 wallOriginal = new Vector3(wall.transform.position.x, -20, wall.transform.position.z);
                    Vector3 wallUp = new Vector3(wall.transform.position.x, 7, wall.transform.position.z);

                    wall.transform.position = Vector3.Lerp(wallUp, wallOriginal, currentDistance / distanceStart);
                }
            }
        }
    }
}

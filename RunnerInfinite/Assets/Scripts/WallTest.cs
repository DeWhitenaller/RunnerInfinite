using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    public GameObject player;
    public GameObject wall;
    public float distance;
    Vector3 wallOriginal;
    // Start is called before the first frame update
    void Start()
    {
        wallOriginal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = player.transform.position.z - wall.transform.position.z;

        if (currentDistance < distance)
        {
            Vector3 wallUp = new Vector3(wall.transform.position.x, wallOriginal.y + 10, wall.transform.position.z);

            wall.transform.position = Vector3.Lerp(wallUp, wallOriginal, currentDistance / distance);
        }
    }
}

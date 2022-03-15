using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTrigger : MonoBehaviour
{
    Transform player;
    PlayerMove playerMove;
    bool changed = false;
    [SerializeField] bool left, right;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMove = player.GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (changed) return;

        if(Vector3.Distance(player.position, transform.position) < 70f)
        {
            playerMove.directionCanBeRequested = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            changed = true;

            if(left) playerMove.ChangeDirection("left");
            else if(right) playerMove.ChangeDirection("right");
            else playerMove.ChangeDirection("both");
        }
    }
}

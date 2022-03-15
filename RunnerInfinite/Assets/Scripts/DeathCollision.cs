using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    PlayerMove playerMove;
    void Start()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && playerMove.directionChangeFailed)
        {
            Debug.Log("dead");
        }
    }
}

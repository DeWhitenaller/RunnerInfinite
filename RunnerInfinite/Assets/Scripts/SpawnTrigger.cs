using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] TileSpawnManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("TileSpawnManager").GetComponent<TileSpawnManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.SpawnTile();
        }
    }
}

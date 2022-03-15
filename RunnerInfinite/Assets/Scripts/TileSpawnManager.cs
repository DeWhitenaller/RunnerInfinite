using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileSpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> tilePrefabs;
    [SerializeField] float moveUpTime;
    [SerializeField] List<GameObject> tilesOnMap;
    [SerializeField] Transform rotationWatcher;
    [SerializeField] GameManager gameManager;

    public void SpawnTile()
    {
        GameObject tile = InstantiateTile();

        GameObject currentTile = tilesOnMap[tilesOnMap.Count - 1];
        tilesOnMap[0].GetComponent<WallManager>().enabled = false;
        tilesOnMap[0].GetComponent<ObstacleSpawnManager>().enabled = false;

        tile.transform.position = currentTile.transform.position;
        tile.transform.rotation = rotationWatcher.rotation;
        tile.transform.position += rotationWatcher.transform.forward * 100;

        tile.transform.DOMoveY(0, moveUpTime);
        tilesOnMap.Add(tile);

        if(tilesOnMap.Count > 3)
        {
            Destroy(tilesOnMap[0]);
            tilesOnMap.RemoveAt(0);
        }
    }

    GameObject InstantiateTile()
    {
        if (gameManager.isInHyperDrive)
        {
            GameObject tile = Instantiate(tilePrefabs[0]);
            return tile;
        }
        else
        {
            GameObject tile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Count)]);
            return tile;
        }
    }
}

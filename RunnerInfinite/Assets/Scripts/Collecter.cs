using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void CollectItem(int id)
    {
        switch (id)
        {
            case 1:
                Debug.Log("Coin");
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.Log("Item has no ID");
                break;
        }
    }
}

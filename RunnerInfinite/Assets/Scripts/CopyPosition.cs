using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] GameObject objectToCopyFrom;
    [SerializeField] float offset;

    void Start()
    {
        offset = transform.position.y - objectToCopyFrom.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, objectToCopyFrom.transform.position.y + offset, transform.position.z);
    }
}

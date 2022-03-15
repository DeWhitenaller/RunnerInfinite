using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBase : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    public int id;

    public virtual void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        meshRenderer.enabled = false;
        other.gameObject.GetComponent<Collecter>().CollectItem(id);
    }

    public virtual void OnCollect(GameObject player)
    {

    }
}

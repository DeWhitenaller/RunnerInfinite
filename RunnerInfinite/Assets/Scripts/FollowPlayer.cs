using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        speed = playerMove.speed;

        transform.rotation = player.rotation;
        //transform.localPosition = new Vector3(player.transform.position.x, transform.localPosition.y, transform.localPosition.z);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void ResetToPlayerPosition()
    {
        transform.position = player.position;
    }
}

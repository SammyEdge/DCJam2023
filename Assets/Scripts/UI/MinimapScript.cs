using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    //public Transform player;
    public GameObject player;

    void Start()
    {
        //player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}

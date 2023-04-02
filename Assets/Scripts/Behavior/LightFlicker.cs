using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private bool Fliper = false;
    private Vector3 DefaultPositon;
    // Start is called before the first frame update
    void Start()
    {
        DefaultPositon = gameObject.transform.position;
        InvokeRepeating("Jerking", 0, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Fliper)
        //{
        //    gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-0.2f, 0.2f), gameObject.transform.position.y + Random.Range(-0.2f, 0.2f), gameObject.transform.position.z + Random.Range(-0.2f, 0.2f));
        //    Fliper = false;
        //}
        //else
        //{
        //    gameObject.transform.position = DefaultPositon;
        //    Fliper = true;
        //}
    }

    void Jerking()
    {
        if (Fliper)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-0.2f, 0.2f), gameObject.transform.position.y + Random.Range(-0.2f, 0.2f), gameObject.transform.position.z + Random.Range(-0.2f, 0.2f));
            Fliper = false;
        }
        else
        {
            gameObject.transform.position = DefaultPositon;
            Fliper = true;
        }
    }
}

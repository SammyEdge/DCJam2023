using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitKeysAction : MonoBehaviour
{
    private GameObject Utils;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Utils = GameObject.FindGameObjectWithTag("Utils");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        Utils.GetComponent<Utils>().UpdateCursor(gameObject.transform.parent.gameObject, CursorAction.Use);
    }

    public void OnMouseExit()
    {
        Utils.GetComponent<Utils>().UpdateCursor(gameObject.transform.parent.gameObject);
    }
    public void OnMouseOver()
    {
        if (!Player.GetComponent<PlayerMovement>().moving.Current)
        {
            Utils.GetComponent<Utils>().UpdateCursor(gameObject.transform.parent.gameObject, CursorAction.Use);
        }
    }
}

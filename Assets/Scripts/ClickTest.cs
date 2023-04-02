using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        print("OnMouseDown");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print(gameObject.transform.name);
        //Destroy(gameObject);
    }
}

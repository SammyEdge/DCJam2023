using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject LeftBag;
    public GameObject RightBag;
    private float mouseYPrev;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //4oto ne tak, camera vse ravno krutitsa dalshe
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        //print(mouseY);
        transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 60f), mouseX * 50f, transform.localRotation.z));

        //if (mouseY < -0.35)
        //{
        //    LeftBag.SetActive(true);
        //    RightBag.SetActive(true);            
        //}
        //else
        //{
        //    LeftBag.SetActive(false);
        //    RightBag.SetActive(false);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenController : MonoBehaviour
{
    public GameObject RenderTexture;
    [SerializeField] private int ScreenDivider;
    [SerializeField] private RenderTexture PSXTexture;
    private float Fix;
    // Start is called before the first frame update
    void Start()
    {
        //print(PSXTexture.height);
        //PSXTexture.height = Screen.height/6;
        //PSXTexture.width = Screen.width / 6;
        //print(PSXTexture.height);
        ////print(Screen.width);
        ////Fix = (float)Screen.height/ (float)Screen.width;
        //RenderTexture.transform.localScale = new Vector3((float)Screen.width/ ScreenDivider, (float)Screen.height / ScreenDivider, 0);
        //print(RenderTexture.transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        //RenderTexture.transform.localScale = new Vector3((float)Screen.width / ScreenDivider, (float)Screen.height / ScreenDivider, 0);
    }
}

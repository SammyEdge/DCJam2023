using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStuff : MonoBehaviour
{
    public bool Torch;
    public GameObject TorchObject;
    public bool Wall;
    public GameObject WallObject;
    public bool WallConnector;
    public GameObject WallConnectorObject;
    public GameObject OppositWallTorch;
    public int hp = 3;
    public bool breakable;
    public Material WallMaterialDefault;
    public Material WallMaterialBreakable;

        // Start is called before the first frame update
    void Start()
    {
        //if (Random.Range(0, 2) == 0)
        //{
        //    Torch.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}

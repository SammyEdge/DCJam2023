using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStuff : MonoBehaviour
{
    public bool Torch;
    public GameObject TorchObject;
    public GameObject TorchObjectPast;
    public GameObject TorchObjectFuture;
    public bool Wall;
    public GameObject WallObject;
    public bool WallConnector;
    public GameObject WallConnectorObject;
    public GameObject OppositWallTorch;
    public int hp = 3;
    public bool breakable;
    public Material WallMaterialDefault;
    public Material WallMaterialOld;
    public Material WallMaterialBreakable;
    public Material WallMaterialBreakableOld;
    public Material ConnectorMaterialDefault;
    public Material ConnectorMaterialOld;

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

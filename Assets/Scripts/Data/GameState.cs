using System.Net.Mime;
//using System.ComponentModel.DataAnnotations;
using System;
//using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Utils;

    public TextMeshProUGUI timeStateText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;

    public Image redKey, blueKey;

    public TMP_FontAsset originalFont, shiftedFont;


    public TimeState timeState;

    public GameObject[] ActiveTiles;
    // Start is called before the first frame update
    void Start()
    {
        redKey.enabled = false;
        blueKey.enabled = false;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(Utils);
        timeState = TimeState.Original;
        Player.GetComponent<PlayerStats>().timeState = TimeState.Original;
        timeStateText.text = Enum.GetName(typeof(TimeState), TimeState.Original);
        ChangeUI(timeState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<PlayerStats>().shiftCooldown)
        {   
            energyText.faceColor = Color.red;
        }
        else
        {
            if (!energyText.faceColor.Equals(Color.white))
            {
                energyText.faceColor = Color.white;
            }
        }
        

        if (Input.GetKeyDown("space"))
        {
            //ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
            //print(ActiveTiles);
            if (timeState == TimeState.Original)
            {
                if (Player.GetComponent<PlayerStats>().shiftCooldown)
                {
                    // сыграть звук пшшшш
                    return;
                }

                //timeState = TimeState.Shifted;
                ShiftTimeState(TimeState.Shifted);
                // foreach (GameObject tile in ActiveTiles)
                // {
                //     foreach(GameObject TileWall in tile.GetComponent<MazeTile>().WallsObjects)
                //     {
                //         //print(TileWall.GetComponent<WallStuff>().WallConnectorObject.transform.name);
                //         //print(TileWall.transform.position);
                //         if (TileWall.GetComponent<WallStuff>().Wall)
                //         {
                //             if (TileWall.GetComponent<WallStuff>().breakable)
                //             {
                //                 TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialBreakableOld;
                //             }
                //             else
                //             {
                //                 TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialOld;
                //             }
                //             if (TileWall.GetComponent<WallStuff>().Torch)
                //             {
                //                 TileWall.GetComponent<WallStuff>().TorchObjectPast.SetActive(true);
                //                 TileWall.GetComponent<WallStuff>().TorchObjectFuture.SetActive(false);
                //                 TileWall.GetComponent<WallStuff>().TorchObject = TileWall.GetComponent<WallStuff>().TorchObjectPast;
                //             }
                //         }
                //         if (TileWall.GetComponent<WallStuff>().WallConnectorObject)
                //         {
                //             TileWall.GetComponent<WallStuff>().WallConnectorObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().ConnectorMaterialOld;
                //         }
                //     }
                //     tile.GetComponent<MazeTile>().Ceiling.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().CeilingPast;
                //     tile.GetComponent<MazeTile>().Floor.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().FloorPast;
                // }
                //ChangeUI(TimeState.Shifted);
            }
            else
            {
                //timeState = TimeState.Original;
                ShiftTimeState(TimeState.Original);
                Player.GetComponent<PlayerStats>().ShiftCooldown();
                // foreach (GameObject tile in ActiveTiles)
                // {
                //     foreach (GameObject TileWall in tile.GetComponent<MazeTile>().WallsObjects)
                //     {
                //         if (TileWall.GetComponent<WallStuff>().Wall)
                //         {
                //             if (TileWall.GetComponent<WallStuff>().breakable)
                //             {
                //                 TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialBreakable;
                //             }
                //             else
                //             {
                //                 TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialDefault;
                //             }
                //             if (TileWall.GetComponent<WallStuff>().Torch)
                //             {
                //                 TileWall.GetComponent<WallStuff>().TorchObjectFuture.SetActive(true);
                //                 TileWall.GetComponent<WallStuff>().TorchObjectPast.SetActive(false);
                //                 TileWall.GetComponent<WallStuff>().TorchObject = TileWall.GetComponent<WallStuff>().TorchObjectFuture;
                //             }
                //         }
                //         if (TileWall.GetComponent<WallStuff>().WallConnectorObject)
                //         {
                //             TileWall.GetComponent<WallStuff>().WallConnectorObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().ConnectorMaterialDefault;
                //         }
                //     }
                //     tile.GetComponent<MazeTile>().Ceiling.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().CeilingFuture;
                //     tile.GetComponent<MazeTile>().Floor.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().FloorFuture;
                // }
            }
            //SceneManager.LoadScene("City", LoadSceneMode.Single);

        }
    }

    private void ChangeUI(TimeState state)
    {
        if (state == TimeState.Original)
        {
            timeStateText.font = originalFont;
            healthText.font = originalFont;
            energyText.font = originalFont;
        }
        else
        {
            timeStateText.font = shiftedFont;
            healthText.font = shiftedFont;
            energyText.font = shiftedFont;
        }
    }

    public void ShiftTimeState(TimeState state)
    {
        timeState = state;
        Player.GetComponent<PlayerStats>().timeState = state;
        timeStateText.text = Enum.GetName(typeof(TimeState), state);
        ChangeUI(state);

        // Monsters
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            //monster.GetComponent<Shiftable>().Shift();
            monster.GetComponent<MonsterController>().Shift();
        }

        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");

        if (state == TimeState.Shifted)
        {
            foreach (GameObject tile in ActiveTiles)
            {
                foreach (GameObject TileWall in tile.GetComponent<MazeTile>().WallsObjects)
                {
                    //print(TileWall.GetComponent<WallStuff>().WallConnectorObject.transform.name);
                    //print(TileWall.transform.position);
                    if (TileWall.GetComponent<WallStuff>().Wall)
                    {
                        if (TileWall.GetComponent<WallStuff>().breakable)
                        {
                            TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialBreakableOld;
                        }
                        else
                        {
                            TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialOld;
                        }
                        if (TileWall.GetComponent<WallStuff>().Torch)
                        {
                            TileWall.GetComponent<WallStuff>().TorchObjectPast.SetActive(true);
                            TileWall.GetComponent<WallStuff>().TorchObjectFuture.SetActive(false);
                            TileWall.GetComponent<WallStuff>().TorchObject = TileWall.GetComponent<WallStuff>().TorchObjectPast;
                        }
                    }
                    if (TileWall.GetComponent<WallStuff>().WallConnectorObject)
                    {
                        TileWall.GetComponent<WallStuff>().WallConnectorObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().ConnectorMaterialOld;
                    }
                }
                tile.GetComponent<MazeTile>().Ceiling.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().CeilingPast;
                tile.GetComponent<MazeTile>().Floor.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().FloorPast;
                tile.GetComponent<MazeTile>().Shift();
            }
        }
        else
        {
            foreach (GameObject tile in ActiveTiles)
            {
                foreach (GameObject TileWall in tile.GetComponent<MazeTile>().WallsObjects)
                {
                    if (TileWall.GetComponent<WallStuff>().Wall)
                    {
                        if (TileWall.GetComponent<WallStuff>().breakable)
                        {
                            TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialBreakable;
                        }
                        else
                        {
                            TileWall.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().WallMaterialDefault;
                        }
                        if (TileWall.GetComponent<WallStuff>().Torch)
                        {
                            TileWall.GetComponent<WallStuff>().TorchObjectFuture.SetActive(true);
                            TileWall.GetComponent<WallStuff>().TorchObjectPast.SetActive(false);
                            TileWall.GetComponent<WallStuff>().TorchObject = TileWall.GetComponent<WallStuff>().TorchObjectFuture;
                        }
                    }
                    if (TileWall.GetComponent<WallStuff>().WallConnectorObject)
                    {
                        TileWall.GetComponent<WallStuff>().WallConnectorObject.GetComponent<Renderer>().material = TileWall.GetComponent<WallStuff>().ConnectorMaterialDefault;
                    }
                }
                tile.GetComponent<MazeTile>().Ceiling.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().CeilingFuture;
                tile.GetComponent<MazeTile>().Floor.GetComponent<Renderer>().material = tile.GetComponent<MazeTile>().FloorFuture;
                tile.GetComponent<MazeTile>().Shift();
            }
        }

        /*GameObject[] tiles = GameObject.FindGameObjectsWithTag("MazeTile");
        foreach (GameObject tile in tiles)
        {
            //tile.GetComponent<Shiftable>().Shift();
            tile.GetComponent<MazeTile>().Shift();
        }*/
        // Objects
    }

}

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
    [SerializeField] private GameObject MazeCtrlr;

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


        if (Input.GetKeyDown("space") && !Player.GetComponent<PlayerMovement>().trueMove)
        {
            if (Player.GetComponent<PlayerStats>().energy <= 0)
            {
                // no energy no shift
                return;
            }

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
            timeStateText.fontSize = 20;
            healthText.font = originalFont;
            healthText.fontSize = 13;
            energyText.font = originalFont;
            energyText.fontSize = 13;
        }
        else
        {
            timeStateText.font = shiftedFont;
            timeStateText.fontSize = 36;
            healthText.font = shiftedFont;
            healthText.fontSize = 20;
            energyText.font = shiftedFont;
            energyText.fontSize = 20;
        }
    }

    public void ShiftTimeState(TimeState state)
    {
        timeState = state;
        Player.GetComponent<PlayerStats>().timeState = state;
        timeStateText.text = Enum.GetName(typeof(TimeState), state);
        ChangeUI(state);

        // Monsters
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        List<GameObject> monsters = MazeCtrlr.GetComponent<LabirintCreation>().Monsters;

        foreach (GameObject monster in monsters)
        {
            //monster.GetComponent<Shiftable>().Shift();
            //monster.GetComponent<MonsterController>().Shift();
            //enable or disable monsters
            if (monster.GetComponent<MonsterController>().timeState != state)
            {
                monster.SetActive(false);
            }
            else
            {
                monster.SetActive(true);
            }
        }

        monsters = MazeCtrlr.GetComponent<LabirintCreation>().MonstersShifted;
        foreach (GameObject monster in monsters)
        {
            //enable or disable monsters
            if (monster.GetComponent<MonsterController>().timeState != state)
            {
                monster.SetActive(false);
            }
            else
            {
                monster.SetActive(true);
            }
        }

        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");

        if (state == TimeState.Shifted)
        {
            // Boost player speed
            Player.GetComponent<PlayerMovement>().Speed += 3;
            Player.GetComponent<PlayerMovement>().RotationSpeed += 30;

            // Shift occupation
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, true, state);
            //print("set occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + state);
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, false, TimeState.Original);
            //print("remove occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + TimeState.Original);

            // Shift textures
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
            // Boost player speed
            Player.GetComponent<PlayerMovement>().Speed -= 3;
            Player.GetComponent<PlayerMovement>().RotationSpeed -= 30;

            // Shift occupation
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, true, state);
            //print("set occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + state);
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, false, TimeState.Shifted);
            //print("remove occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + TimeState.Shifted);

            // Shift textures
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

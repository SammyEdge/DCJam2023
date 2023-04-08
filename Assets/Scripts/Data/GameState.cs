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
using System.Linq;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Utils;
    [SerializeField] private GameObject MazeCtrlr;

    public TextMeshProUGUI timeStateText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI logText;

    public Image redKey, blueKey;

    public TMP_FontAsset originalFont, shiftedFont;

    public TimeState timeState;

    public GameObject[] ActiveTiles;

    // Sound engine
    public AudioSource sound;
    public AudioClip originalMusic;
    public AudioClip shiftedMusic;

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

        sound = gameObject.transform.GetComponent<AudioSource>();
        sound.clip = originalMusic;
        sound.Play();
        
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
                Player.GetComponent<PlayerSoundController>().BumpSound();
                // no energy no shift
                return;
            }

            
            //ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
            //print(ActiveTiles);
            if (timeState == TimeState.Original)
            {
                if (Player.GetComponent<PlayerStats>().shiftCooldown)
                {
                    Player.GetComponent<PlayerLogController>().Message("Need some time to cool your Shifter");
                    // сыграть звук пшшшш
                    Player.GetComponent<PlayerSoundController>().CooldownSound();
                    return;
                }

                //timeState = TimeState.Shifted;
                Player.GetComponent<PlayerLogController>().Message("BACK IN TIME!");
                Player.GetComponent<PlayerSoundController>().ShiftSound();
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
                Player.GetComponent<PlayerLogController>().Message("Shited Back!");
                Player.GetComponent<PlayerSoundController>().UnShiftSound();
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
            logText.font = originalFont;
            logText.fontSize = 13;
        }
        else
        {
            timeStateText.font = shiftedFont;
            timeStateText.fontSize = 36;
            healthText.font = shiftedFont;
            healthText.fontSize = 20;
            energyText.font = shiftedFont;
            energyText.fontSize = 20;
            logText.font = shiftedFont;
            logText.fontSize = 20;
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
            sound.clip = shiftedMusic;
            sound.Play();

            GameObject Exit = GameObject.FindWithTag("Exit");
            Exit.GetComponent<MeshFilter>().mesh = Exit.GetComponent<ExitContoller>().Past;
            Exit.GetComponent<ExitContoller>().KeyFutureButton.SetActive(true);
            Exit.GetComponent<ExitContoller>().KeyPastButton.SetActive(true);
            if (Exit.GetComponent<ExitContoller>().KeyPastInserted)
            {
                Exit.GetComponent<ExitContoller>().KeyPast.SetActive(true);
            }
            if (Exit.GetComponent<ExitContoller>().KeyFutureInserted)
            {
                Exit.GetComponent<ExitContoller>().KeyFuture.SetActive(true);
            }

            // Boost player speed
            Player.GetComponent<PlayerMovement>().Speed += 3;
            Player.GetComponent<PlayerMovement>().RotationSpeed += 30;

            // Shift occupation
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, true, state);
            //print("set occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + state);
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, false, TimeState.Original);
            //print("remove occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + TimeState.Original);

            GameObject boner = Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().MonstersShifted.FirstOrDefault(monster => monster.transform.position == Player.transform.position);
            if (boner != null)
            {
                boner.GetComponent<MonsterController>().DropLoot();
                boner.GetComponent<MonsterController>().MonsterDie();
            }

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

            // Shift portal

        }
        else
        {
            sound.clip = originalMusic;
            sound.Play();
            
            GameObject Exit = GameObject.FindWithTag("Exit");
            Exit.GetComponent<MeshFilter>().mesh = Exit.GetComponent<ExitContoller>().Future;
            Exit.GetComponent<ExitContoller>().KeyFutureButton.SetActive(false);
            Exit.GetComponent<ExitContoller>().KeyPastButton.SetActive(false);
            Exit.GetComponent<ExitContoller>().KeyPast.SetActive(false);
            Exit.GetComponent<ExitContoller>().KeyFuture.SetActive(false);
            // Boost player speed
            Player.GetComponent<PlayerMovement>().Speed -= 3;
            Player.GetComponent<PlayerMovement>().RotationSpeed -= 30;

            // Shift occupation
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, true, state);
            //print("set occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + state);
            Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().SetOccupation(Player.transform.position, false, TimeState.Shifted);
            //print("remove occupation on " + Player.transform.position.x + ", " + Player.transform.position.z + " in " + TimeState.Shifted);

            // Destroy Boner in that place
            GameObject boner = Player.GetComponent<PlayerMovement>().MazeController.GetComponent<LabirintCreation>().Monsters.FirstOrDefault(monster => monster.transform.position == Player.transform.position);
            if (boner != null)
            {
                boner.GetComponent<MonsterController>().DropLoot();
                boner.GetComponent<MonsterController>().MonsterDie();
            }

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

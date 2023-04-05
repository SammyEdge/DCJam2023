using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ActiveTiles;
    public GameObject[] EnterSouth;
    public GameObject[] EnterWest;
    public GameObject[] EnterEast;
    public GameObject[] EnterNorth;
    public GameObject StartTile;
    private int TilesCounter = 0;
    //private int OpenExitCounter = 0;
    public int MapSize;
    public int IndexOfTile;
    public int WallIndex;
    /*public GameObject[,,] Coordinates;*/
    //public string[,,] Coordinates;
    //private List<GameObject> Coordinates;
    private List<GameObject> Tiles;
    private List<Vector3> Coordinates;
    void Start()
    {
        Coordinates = new List<Vector3>();
        Tiles = new List<GameObject>();
        Coordinates.Add(StartTile.transform.position);
        Tiles.Add(StartTile);
        for (int i = 0; i < MapSize; i++)
        {
            ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
            foreach (GameObject tile in ActiveTiles)
            {
                if (tile.GetComponent<MazeTile>().Exit[0] && TilesCounter < MapSize)
                {
                    if (!Coordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10)))
                    {
                        tile.GetComponent<MazeTile>().Exit[0] = false;
                        GameObject NewlyInstatniated = Instantiate(EnterSouth[Random.Range(0, EnterSouth.Length)], new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10), Quaternion.identity);
                        Coordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = false;
                        TilesCounter++;
                        WallsFix(Tiles, Coordinates, NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[1] && TilesCounter < MapSize)
                {
                    if (!Coordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z - 10)))
                    {
                        tile.GetComponent<MazeTile>().Exit[1] = false;
                        GameObject NewlyInstatniated = Instantiate(EnterNorth[Random.Range(0, EnterNorth.Length)], new Vector3(tile.transform.position.x, 0, tile.transform.position.z - 10), Quaternion.identity);
                        Coordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = false;
                        TilesCounter++;
                        WallsFix(Tiles, Coordinates, NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[2] && TilesCounter < MapSize)
                {
                    if (!Coordinates.Contains(new Vector3(tile.transform.position.x - 10, 0, tile.transform.position.z)))
                    {
                        tile.GetComponent<MazeTile>().Exit[2] = false;
                        GameObject NewlyInstatniated = Instantiate(EnterEast[Random.Range(0, EnterEast.Length)], new Vector3(tile.transform.position.x - 10, 0, tile.transform.position.z), Quaternion.identity);
                        Coordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = false;
                        TilesCounter++;
                        WallsFix(Tiles, Coordinates, NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[3] && TilesCounter < MapSize)
                {
                    if (!Coordinates.Contains(new Vector3(tile.transform.position.x + 10, 0, tile.transform.position.z)))
                    {
                        tile.GetComponent<MazeTile>().Exit[3] = false;
                        GameObject NewlyInstatniated = Instantiate(EnterWest[Random.Range(0, EnterWest.Length)], new Vector3(tile.transform.position.x + 10, 0, tile.transform.position.z), Quaternion.identity);
                        Coordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = false;
                        TilesCounter++;
                        WallsFix(Tiles, Coordinates, NewlyInstatniated);
                    }
                }
                if (TilesCounter >= MapSize)
                {
                    if (tile.GetComponent<MazeTile>().Exit[0])
                    {
                        if (!Coordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10)))
                        {
                            tile.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                        }
                        else
                        {
                        }
                        tile.GetComponent<MazeTile>().Exit[0] = false;
                    }
                    if (tile.GetComponent<MazeTile>().Exit[1])
                    {
                        if (!Coordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z - 10)))
                        {
                            tile.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                        }
                        else
                        {
                        }
                        tile.GetComponent<MazeTile>().Exit[1] = false;
                    }
                    if (tile.GetComponent<MazeTile>().Exit[2])
                    {
                        if (!Coordinates.Contains(new Vector3(tile.transform.position.x - 10, 0, tile.transform.position.z)))
                        {
                            tile.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                        }
                        else
                        {
                        }
                        tile.GetComponent<MazeTile>().Exit[2] = false;
                    }
                    if (tile.GetComponent<MazeTile>().Exit[3])
                    {
                        if (!Coordinates.Contains(new Vector3(tile.transform.position.x + 10, 0, tile.transform.position.z)))
                        {
                            tile.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                        }
                        else
                        {
                        }
                        tile.GetComponent<MazeTile>().Exit[3] = false;
                    }
                }
            }
        }
        Debug.Log(Coordinates);
        foreach (var Coordinate in Coordinates)
        {
            //print(Coordinate);
        }
    }
    void WallsFix(List<GameObject> Tiles, List<Vector3> Coordinates, GameObject NewlyInstatniated)
    {
        WallIndex = 0;
        foreach (var Wall in NewlyInstatniated.GetComponent<MazeTile>().Walls)
        {
            if (Wall)
            {
                //print(WallIndex);
                if (WallIndex == 0)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z + 10));
                    if (IndexOfTile > 0)
                    {
                        Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                        Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[1] = true;
                    }
                }
                if (WallIndex == 1)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z - 10));
                    if (IndexOfTile > 0)
                    {
                        Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                        Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[0] = true;
                    }
                }
                if (WallIndex == 2)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x - 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                        Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[3] = true;
                    }
                }
                if (WallIndex == 3)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x + 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                        Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[2] = true;
                    }
                }
            }
            else
            {
                if (WallIndex == 0)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z + 10));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[1])
                        {
                            NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                            NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                        }
                    }
                }
                if (WallIndex == 1)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z - 10));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[0])
                        {
                            NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                            NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                        }
                    }
                }
                if (WallIndex == 2)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x - 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[3])
                        {
                            NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                            NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                        }
                    }
                }
                if (WallIndex == 3)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x + 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[2])
                        {
                            NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                            NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                        }
                    }
                }
            }
            WallIndex++;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

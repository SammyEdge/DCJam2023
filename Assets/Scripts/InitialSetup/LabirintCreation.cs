using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LabirintCreation : MonoBehaviour
{
    public GameObject EnterBlock;
    public GameObject BlockBehindEnter;
    public GameObject StartTile;
    public GameObject RawTile;
    public GameObject Chest;
    public GameObject Monster;
    public GameObject[] ActiveTiles;
    public List<GameObject> Tiles;
    public List<Vector3> TilesCoordinates;
    public List<Vector3> ConnectorCoordinates;
    public List<Vector3> ConnectorCoordinatesNoDup; //�������� �� ������ ��� ������
    public List<Vector3> WallsCoordinates;
    public List<Vector3> WallsCoordinatesNoDupes; //�������� �� ������ ��� ������
    public List<Vector3> BreakableWalls; //�������� �� ������ ��� ������
    public int MapSize;
    public int SideOfNewTile;
    public int IndexOfTile;
    public int TorchChecker = 0;
    public int TorchesCounter = 0;
    public Material BreakableMaterial;
    private WallSides wallSides = new WallSides();
    // Start is called before the first frame update
    void Start()
    {
        TilesCoordinates = new List<Vector3>();
        ConnectorCoordinates = new List<Vector3>();
        ConnectorCoordinatesNoDup = new List<Vector3>();
        WallsCoordinates = new List<Vector3>();
        WallsCoordinatesNoDupes = new List<Vector3>();
        BreakableWalls = new List<Vector3>();
        Tiles = new List<GameObject>();
        TilesCoordinates.Add(StartTile.transform.position);
        TilesCoordinates.Add(BlockBehindEnter.transform.position);
        TilesCoordinates.Add(EnterBlock.transform.position);
        Tiles.Add(StartTile);
        Tiles.Add(BlockBehindEnter);
        Tiles.Add(EnterBlock);

        GameObject[] StartingWalls = GameObject.FindGameObjectsWithTag("StartingWall");
        foreach (GameObject wall in StartingWalls)
        {
            WallsCoordinates.Add(wall.transform.position);
        }

        GameObject[] StartingConnecters = GameObject.FindGameObjectsWithTag("StartingConnecter");
        foreach (GameObject connecter in StartingConnecters)
        {
            ConnectorCoordinates.Add(connecter.transform.position);
        }

        for (int i = 0; i < MapSize; i++)
        {
            ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
            foreach (GameObject tile in ActiveTiles)
            {
                if (tile.GetComponent<MazeTile>().Exit[0])
                {
                    if (!TilesCoordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10)))
                    {
                        GameObject NewlyInstatniated = Instantiate(RawTile, new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10), Quaternion.identity);
                        TilesCoordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = false;
                        MoldTilev2(wallSides.South, NewlyInstatniated);
                        WallsFix(Tiles, TilesCoordinates, NewlyInstatniated);
                        PlaceTorch(NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[1])
                {
                    if (!TilesCoordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z - 10)))
                    {
                        GameObject NewlyInstatniated = Instantiate(RawTile, new Vector3(tile.transform.position.x, 0, tile.transform.position.z - 10), Quaternion.identity);
                        TilesCoordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = false;
                        MoldTilev2(wallSides.North, NewlyInstatniated);
                        WallsFix(Tiles, TilesCoordinates, NewlyInstatniated);
                        PlaceTorch(NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[2])
                {
                    if (!TilesCoordinates.Contains(new Vector3(tile.transform.position.x - 10, 0, tile.transform.position.z)))
                    {
                        GameObject NewlyInstatniated = Instantiate(RawTile, new Vector3(tile.transform.position.x - 10, 0, tile.transform.position.z), Quaternion.identity);
                        TilesCoordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = false;
                        MoldTilev2(wallSides.East, NewlyInstatniated);
                        WallsFix(Tiles, TilesCoordinates, NewlyInstatniated);
                        PlaceTorch(NewlyInstatniated);
                    }
                }
                if (tile.GetComponent<MazeTile>().Exit[3])
                {
                    if (!TilesCoordinates.Contains(new Vector3(tile.transform.position.x + 10, 0, tile.transform.position.z)))
                    {
                        GameObject NewlyInstatniated = Instantiate(RawTile, new Vector3(tile.transform.position.x + 10, 0, tile.transform.position.z), Quaternion.identity);
                        TilesCoordinates.Add(NewlyInstatniated.transform.position);
                        Tiles.Add(NewlyInstatniated);
                        NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = false;
                        MoldTilev2(wallSides.West, NewlyInstatniated);
                        WallsFix(Tiles, TilesCoordinates, NewlyInstatniated);
                        PlaceTorch(NewlyInstatniated);
                    }
                }
            }
        }
        CLoseLabirint(TilesCoordinates);
        PlaceBreakableWall();
        PlaceChests();
        PlaceMonsters(Monster, 25); // Only 5 monsters now, you can increase, decrease or randomize this qty
    }
    /*void MoldTile(int EnterPoint, GameObject NewlyInstatniated)
    {
        if (EnterPoint == 1)
        {
            int TileTypeId = Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 0)
        {
            int TileTypeId = Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 3)
        {
            int TileTypeId = Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 2)
        {
            int TileTypeId = Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
    }*/

    void MoldTilev2(int EnterPoint, GameObject NewlyInstatniated)
    {
        if (EnterPoint == 1)
        {
            int TileTypeId = UnityEngine.Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);

                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);

                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 0)
        {
            int TileTypeId = UnityEngine.Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);   
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);   
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 3)
        {
            int TileTypeId = UnityEngine.Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);  
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);

            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
        if (EnterPoint == 2)
        {
            int TileTypeId = UnityEngine.Random.Range(0, 7);
            if (TileTypeId == 0)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 1)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 2)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 3)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 4)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 5)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                CreateTileDefaultWalls(NewlyInstatniated);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
            }
            if (TileTypeId == 6)
            {
                NewlyInstatniated.GetComponent<MazeTile>().Exit[0] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[1] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[2] = true;
                NewlyInstatniated.GetComponent<MazeTile>().Exit[3] = true;
            }
        }
    }

    void CreateTileDefaultWalls(GameObject NewlyInstatniated)
    {
        int SidesIndexCounter = 0;
        foreach (var item in NewlyInstatniated.GetComponent<MazeTile>().Exit)
        {
            if (!item)
            {
                if (!WallsCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallObject.transform.position))
                {
                    NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().Wall = true;
                    NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallObject.SetActive(true);
                    WallsCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallObject.transform.position);
                }
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().Wall = true;
                //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallObject.SetActive(true);
                if (!ConnectorCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                {
                    NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                    ConnectorCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[SidesIndexCounter].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                }
            }
            SidesIndexCounter++;
        }
    }
    void MoveToAnotherWall(GameObject NewGameObject, GameObject Transfer)
    {

    }
    void WallsFix(List<GameObject> Tiles, List<Vector3> Coordinates, GameObject NewlyInstatniated)
    {
        SideOfNewTile = 0;
        // ������ ������
        //foreach (var Wall in NewlyInstatniated.GetComponent<MazeTile>().Walls)
        //{
        //    if (Wall)
        //    {

        //    }
        //}

        //foreach (var WallObject in NewlyInstatniated.GetComponent<MazeTile>().WallsObjects)
        //{
        //    if (WallObject.GetComponent<WallStuff>().Wall)
        //    {

        //    }
        //}
        foreach (var WallObject in NewlyInstatniated.GetComponent<MazeTile>().WallsObjects)
        {
            if (WallObject.GetComponent<WallStuff>().Wall)
            {
                //print(WallIndex);
                if (SideOfNewTile == wallSides.North)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z + 10));
                    if (IndexOfTile > 0)
                    {
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[1] = true;

                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);

                        //CreateBreakableWall(WallObject);

                        if (!ConnectorCoordinates.Contains(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                        {
                            Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                            ConnectorCoordinates.Add(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                        }
                    }
                }
                if (SideOfNewTile == wallSides.South)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z - 10));
                    if (IndexOfTile > 0)
                    {
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[0] = true;

                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);

                        //CreateBreakableWall(WallObject);

                        if (!ConnectorCoordinates.Contains(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                        {
                            Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                            ConnectorCoordinates.Add(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                        }
                    }
                }
                if (SideOfNewTile == wallSides.West)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x - 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[3] = true;

                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                        //CreateBreakableWall(WallObject);

                        if (!ConnectorCoordinates.Contains(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                        {
                            Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                            ConnectorCoordinates.Add(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                        }
                    }
                }
                if (SideOfNewTile == wallSides.East)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x + 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().Walls[2] = true;

                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                        //Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                        //CreateBreakableWall(WallObject);

                        if (!ConnectorCoordinates.Contains(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                        {
                            Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                            ConnectorCoordinates.Add(Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                        }
                    }
                }
            }
            else
            {
                if (SideOfNewTile == wallSides.North)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z + 10));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall)
                        {
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                            //NewlyInstatniated.GetComponent<MazeTile>().Walls[0] = true;

                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
                            if (!ConnectorCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                            {
                                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                                ConnectorCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                            }
                        }
                    }
                }
                if (SideOfNewTile == wallSides.South)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x, 0, NewlyInstatniated.transform.position.z - 10));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall)
                        {
                            //    NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                            //    NewlyInstatniated.GetComponent<MazeTile>().Walls[1] = true;

                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
                            if (!ConnectorCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                            {
                                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                                ConnectorCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                            }
                        }
                    }
                }
                if (SideOfNewTile == wallSides.West)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x - 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall)
                        {
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                            //NewlyInstatniated.GetComponent<MazeTile>().Walls[2] = true;

                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
                            if (!ConnectorCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                            {
                                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                                ConnectorCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                            }
                        }
                    }
                }
                if (SideOfNewTile == wallSides.East)
                {
                    IndexOfTile = Coordinates.IndexOf(new Vector3(NewlyInstatniated.transform.position.x + 10, 0, NewlyInstatniated.transform.position.z));
                    if (IndexOfTile > 0)
                    {
                        if (Tiles[IndexOfTile].GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall)
                        {
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                            //NewlyInstatniated.GetComponent<MazeTile>().Walls[3] = true;

                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                            //NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                            if (!ConnectorCoordinates.Contains(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.transform.position))
                            {
                                NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.SetActive(true);
                                ConnectorCoordinates.Add(NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallConnectorObject.transform.position);
                            }
                        }
                    }
                }
            }
            SideOfNewTile++;
        }
    }
    // Update is called once per frame
    void CLoseLabirint(List<Vector3> Coordinates)
    {
        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
        foreach (GameObject tile in ActiveTiles)
        {
            if (tile.GetComponent<MazeTile>().Exit[0])
            {
                if (!Coordinates.Contains(new Vector3(tile.transform.position.x, 0, tile.transform.position.z + 10)))
                {
                    //tile.GetComponent<MazeTile>().WallsObjects[0].SetActive(true);
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall = true;
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.SetActive(true);
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
                    //tile.GetComponent<MazeTile>().WallsObjects[1].SetActive(true);
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall = true;
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.SetActive(true);
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
                    //tile.GetComponent<MazeTile>().WallsObjects[2].SetActive(true);
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall = true;
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.SetActive(true);
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
                    //tile.GetComponent<MazeTile>().WallsObjects[3].SetActive(true);
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall = true;
                    tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.SetActive(true);
                }
                else
                {
                }
                tile.GetComponent<MazeTile>().Exit[3] = false;
            }
        }
    }

    void PlaceTorch(GameObject NewlyInstatniated)
    {
        if (TorchChecker == 0)
        {
            var i = 0;
            foreach (var WallObject in NewlyInstatniated.GetComponent<MazeTile>().WallsObjects)
            {
                if (WallObject.GetComponent<WallStuff>().Wall)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        //print(i);
                        //NewlyInstatniated.GetComponent<MazeTile>().WallTorches[i].SetActive(true);
                        //NewlyInstatniated.GetComponent<MazeTile>().Torches[i] = true;
                        NewlyInstatniated.GetComponent<MazeTile>().WallsObjects[i].GetComponent<WallStuff>().Torch = true;
                        TorchesCounter++;
                        break;
                    }
                }
                i++;
            }
            TorchChecker = 1;
        }
        else
        {
            TorchChecker = 0;
        }
    }

    void PlaceBreakableWall()
    {
        // ���� �������� �������� �� ������� ������, ����� �� ���� ���������� ������ �� ������� ���������
        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
        foreach (GameObject tile in ActiveTiles)
        {
            //print(tile.GetComponent<MazeTile>().WallsObjects[0].GetComponent<WallStuff>().Wall);
            if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall && tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    if (!WallsCoordinates.Contains(tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().WallObject.transform.position))
                    {
                        CreateBreakableWall(tile.GetComponent<MazeTile>().WallsObjects[wallSides.West]);
                    }
                }
                else
                {
                    if (!WallsCoordinates.Contains(tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().WallObject.transform.position))
                    {
                        CreateBreakableWall(tile.GetComponent<MazeTile>().WallsObjects[wallSides.East]);
                    }
                }
            }
            if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall && tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    if (!WallsCoordinates.Contains(tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().WallObject.transform.position))
                    {
                        CreateBreakableWall(tile.GetComponent<MazeTile>().WallsObjects[wallSides.North]);
                    }
                }
                else
                {
                    if (!WallsCoordinates.Contains(tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().WallObject.transform.position))
                    {
                        CreateBreakableWall(tile.GetComponent<MazeTile>().WallsObjects[wallSides.South]);
                    }
                }
            }
        }
    }

    void CreateBreakableWall(GameObject WallObject)
    {
        if (!WallObject.GetComponent<WallStuff>().Wall)
        {
            WallObject.GetComponent<WallStuff>().breakable = true;
            WallObject.GetComponent<WallStuff>().Wall = true;
            WallObject.GetComponent<WallStuff>().WallObject.SetActive(true);
            WallObject.GetComponent<WallStuff>().WallObject.GetComponent<Renderer>().material = WallObject.GetComponent<WallStuff>().WallMaterialBreakable;
            BreakableWalls.Add(WallObject.transform.position);
        }
    }

    void PlaceChests()
    {
        // ���� �������� �������� �� ������� ������, ����� �� ���� ���������� ������ �� ������� ���������
        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
        foreach (GameObject tile in ActiveTiles)
        {
            //print(tile.GetComponent<MazeTile>().WallsObjects[0].GetComponent<WallStuff>().Wall);
            if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().Wall && tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().Wall && tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().Wall && tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().Wall)
            {
                if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].GetComponent<WallStuff>().breakable)
                {
                    GameObject NewChest = Instantiate(Chest, tile.transform.position, Quaternion.Euler(0, 180, 0));
                    NewChest.transform.position = NewChest.transform.position + (NewChest.transform.position - new Vector3(tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].transform.position.x, NewChest.transform.position.y, tile.GetComponent<MazeTile>().WallsObjects[wallSides.North].transform.position.z)) / 1.5f;
                }
                if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].GetComponent<WallStuff>().breakable)
                {
                    GameObject NewChest = Instantiate(Chest, tile.transform.position, Quaternion.identity);
                    NewChest.transform.position = NewChest.transform.position + (NewChest.transform.position - new Vector3(tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].transform.position.x, NewChest.transform.position.y, tile.GetComponent<MazeTile>().WallsObjects[wallSides.South].transform.position.z)) / 1.5f;
                }
                if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].GetComponent<WallStuff>().breakable)
                {
                    GameObject NewChest = Instantiate(Chest, tile.transform.position, Quaternion.Euler(0, 90, 0));
                    NewChest.transform.position = NewChest.transform.position + (NewChest.transform.position - new Vector3(tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].transform.position.x, NewChest.transform.position.y, tile.GetComponent<MazeTile>().WallsObjects[wallSides.West].transform.position.z)) / 1.5f;

                }
                if (tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].GetComponent<WallStuff>().breakable)
                {
                    GameObject NewChest = Instantiate(Chest, tile.transform.position, Quaternion.Euler(0, -90, 0));
                    NewChest.transform.position = NewChest.transform.position + (NewChest.transform.position - new Vector3(tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].transform.position.x, NewChest.transform.position.y, tile.GetComponent<MazeTile>().WallsObjects[wallSides.East].transform.position.z)) / 1.5f;
                }
            }
        }
    }

    //void PlaceMonster(MonsterType monsterType, int quantity)
    void PlaceMonsters(GameObject monster, int quantity)
    {
        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
        //foreach (GameObject tile in ActiveTiles)
        if (quantity > ActiveTiles.Count())
        {
            // exit if monsters more than squares
            return;
        }
        List<int> indexes = new List<int>();
        System.Random rnd = new System.Random();
        for (int i = 0; i < quantity; ++i)
        {
            // generates an index of a tile to put a monster there
            int newIndex = rnd.Next(0, ActiveTiles.Count());
            while (indexes.Contains(newIndex))
            {
                newIndex = rnd.Next(0, ActiveTiles.Count());
            }
            indexes.Add(newIndex);

            // put a monster on a tile
            GameObject tile = ActiveTiles[newIndex];
            if (tile.transform.position != new Vector3(0,0,0)) {
                GameObject NewMonster = Instantiate(Monster, tile.transform.position, Quaternion.Euler(0, 0, 0));
                NewMonster.name = Monster.name + i.ToString();
            }
            else
            {
                i--;
            }
        }
    }



    void Update()
    {
        ConnectorCoordinatesNoDup = ConnectorCoordinates.Distinct().ToList();
        WallsCoordinatesNoDupes = WallsCoordinates.Distinct().ToList();
    }
}

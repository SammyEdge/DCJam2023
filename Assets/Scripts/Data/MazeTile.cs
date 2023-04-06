using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazeTile : MonoBehaviour//, Shiftable
{
    // Start is called before the first frame update

    //public bool OpenN;
    //public bool OpenS;
    //public bool OpenW;
    //public bool OpenE;
    //public bool StartN;
    //public bool StartS;
    //public bool StartW;
    //public bool StartE;
    //public bool[] Enter;
    public bool[] Exit;
    public bool[] Walls;
    public bool[] Torches;
    //public GameObject WallNorth;
    //public GameObject WallSouth;
    //public GameObject WallWest;
    //public GameObject WallEast;
    public GameObject[] WallsObjects;
    public GameObject[] WallTorches;
    public Material FloorFuture;
    public Material FloorPast;
    public Material CeilingFuture;
    public Material CeilingPast;
    public GameObject Ceiling;
    public GameObject Floor;

    public string objName;

    public bool occupied;

    //public TimeState timeState { get => this.timeState; set => this.timeState = value; }
    public TimeState timeState;

    void Start()
    {
        timeState = TimeState.Original;
    }

    public void Shift()
    {
        if (this.timeState == TimeState.Original)
        {
            this.timeState = TimeState.Shifted;
        }
        else
        {
            this.timeState = TimeState.Original;
        }
    }

    void Awake()
    {
        /*Debug.print("Awake");*/
    }

    /*public void SetName()
    {
        this.objName = "default";
    }*/
}


/*public interface Wallable
{
    public void SetName();
}*/

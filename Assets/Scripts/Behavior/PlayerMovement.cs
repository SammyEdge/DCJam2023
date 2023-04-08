using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Utils;
    public GameObject[] ActiveTiles;
    public GameObject MazeController;
    public GameObject Torch;
    public List<Vector3> Coordinates;
    public Vector3 CurrentPosition;
    private Vector3 FuturePosition;
    public Quaternion CurrentRotation;
    public Vector3 TargetPosition;
    [SerializeField] private Vector3 PositionBeforeMovementStarts;
    private Quaternion TargetRotation;
    public List<Vector3> CurrentPositionForward;
    public List<Vector3> CurrentPositionLeftForward;
    public List<Vector3> CurrentPositionRightForward;
    public List<Vector3> PositionsToTurnLight;
    public GameObject Camera;
    public int TileIndex;
    [SerializeField] private Vector3 FixedForwardVector;
    [SerializeField] private Vector3 FixedLeftVector;
    //private float ForwardX;
    //private float ForwardZ;
    [SerializeField] public float Speed;
    [SerializeField] public float RotationSpeed; //??????? ? ???????

    private int squareSize = 10;

    // Visible light distance
    public int lightDistance;
    //private float TimePassedFixed = 0;
    //private float TimePassed = 0;
    //private float Test = 0;
    [SerializeField] private List<float> TestList;
    //private int MovingCorrection = 0;

    private KeyBinds keyBinds = new KeyBinds();
    public Moving moving = new Moving();
    private MovingDirection movingDirection = new MovingDirection();
    int movingCounter;

    public AudioSource sound;
    public AudioClip clip;

    //private bool KeyForwardPressed = false;
    //private bool KeyBackwardPressed = false;
    //private bool KeyTurnRightPressed = false;
    //private bool KeyTurnLeftPressed = false;
    //private bool KeyStrafeLeftPressed = false;
    //private bool KeyStrafeRightPressed = false;

    [SerializeField] private bool LightsOn;

    private int CameraShakeUp = 0;

    // movement indicator
    public bool trueMove;


    void Start()
    {
        //(SceneManager.GetActiveScene().name);
        PositionBeforeMovementStarts = transform.position;
        TargetPosition = new Vector3(0, 0, 0);
        TargetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        moving.Current = false;
        movingCounter = 0;
        trueMove = false;
        sound = gameObject.transform.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        //Test++;
        //TimePassedFixed += Time.deltaTime;
        //TestList.Add(TimePassedFixed);
        //print("******");
        //print(Test/TimePassedFixed);
        //print("******");
        if (moving.Current)
        {
            if (CameraShakeUp < 10)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y + 0.05f, Camera.transform.position.z);
                Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x - 0.5f, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z - 0.2f);
                CameraShakeUp++;
            }
            else if (CameraShakeUp >= 10 && CameraShakeUp < 20)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y - 0.05f, Camera.transform.position.z);
                Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x + 0.5f, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z + 0.2f);
                CameraShakeUp++;
            }
            else if (CameraShakeUp >= 20 && CameraShakeUp < 30)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y + 0.05f, Camera.transform.position.z);
                Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x - 0.5f, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z + 0.2f);
                CameraShakeUp++;
            }
            else if (CameraShakeUp >= 30)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y - 0.05f, Camera.transform.position.z);
                Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x + 0.5f, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z - 0.2f);
                CameraShakeUp++;
            }
            if (CameraShakeUp > 42)
            {
                CameraShakeUp = 0;
            }
            //print(Camera.transform.eulerAngles.x);
        }
        //print(moving.Current);

    }
    void Update()
    {
        // gaining energy for movement
        if (Camera.transform.parent.GetComponent<PlayerStats>().timeState == TimeState.Original)
        {
            if (movingCounter >= Camera.transform.parent.GetComponent<PlayerStats>().restoreEnergyCounter)
            {
                Camera.transform.parent.GetComponent<PlayerStats>().ChangeEnergy(1);
                movingCounter = 0;
            }
        }

        if (TargetPosition == transform.position && transform.eulerAngles.y == TargetRotation.eulerAngles.y)
        {
            if (moving.Current)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, 5, Camera.transform.position.z);
                Camera.transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y, 0);
                CameraShakeUp = 0;
                moving.Current = false;
                //moving.Rotation = false;
                movingDirection.Current = movingDirection.Forward;

                // unoccupy PositionBeforeMovementStarts
                if (trueMove)
                {
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(PositionBeforeMovementStarts, false, gameObject.GetComponentInParent<PlayerStats>().timeState);
                    //print("removed occupation on " + PositionBeforeMovementStarts.x + ", " + PositionBeforeMovementStarts.z + " in " + gameObject.GetComponentInParent<PlayerStats>().timeState);
                    trueMove = false;
                    sound.Stop();
                }

                PositionBeforeMovementStarts = transform.position;
                if (SceneManager.GetActiveScene().name == "SampleScene 1")
                {
                    //print("ebaniyrotetogojama");
                    DynamicLIght(movingDirection.Current);
                }
                //DynamicLIght(moving.Current);
            }

            if (Input.GetKey(keyBinds.moveForward) && !StopMovementIfNoEnergy())
            {
                //KeyForwardPressed = false;
                RaycastHit Hit;
                if (!Physics.Raycast(transform.position, transform.forward, out Hit, 8f, 3))
                {

                    TargetPosition = transform.position + Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, 1);

                    // check occupation
                    if (MazeController.GetComponent<LabirintCreation>().GetOccupation(TargetPosition, gameObject.GetComponentInParent<PlayerStats>().timeState))
                    {
                        TargetPosition = transform.position;
                        return;
                    }

                    trueMove = true;

                    clip = gameObject.GetComponent<PlayerSoundController>().walk;
                    sound.clip = clip;
                    sound.Play();

                    PositionBeforeMovementStarts = transform.position;
                    moving.Current = true;
                    // gaining energy for movement
                    if (Camera.transform.parent.GetComponent<PlayerStats>().timeState == TimeState.Original)
                    {
                        movingCounter++;
                        Debug.Log("squares moved " + movingCounter.ToString());
                    }

                    // Occupy TargetPosition
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(TargetPosition, true, gameObject.GetComponentInParent<PlayerStats>().timeState);
                    //print("set occupation on " + TargetPosition.x + ", " + TargetPosition.z + " in " + gameObject.GetComponentInParent<PlayerStats>().timeState);

                    movingDirection.Current = movingDirection.Forward;
                    if (SceneManager.GetActiveScene().name == "SampleScene 1")
                    {
                        DynamicLIght(movingDirection.Current);
                    }
                    //transform.position += new Vector3(ForwardX, 0f, ForwardZ);
                }
                //print(transform.position);
                //print(TargetPosition);
            }
            // Backward move pressed
            if (Input.GetKey(keyBinds.moveBackward) && !StopMovementIfNoEnergy())
            {
                //KeyBackwardPressed = false;
                RaycastHit Hit;
                //if (!Physics.Raycast(transform.position, new Vector3(transform.forward.x * -1, transform.forward.y, transform.forward.z * -1), out Hit, lightDistance))
                if (!Physics.Raycast(transform.position, transform.forward * -1, out Hit, 8f, 3))
                {
                    TargetPosition = transform.position + Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, -1);

                    // check occupation
                    if (MazeController.GetComponent<LabirintCreation>().GetOccupation(TargetPosition, Camera.transform.parent.GetComponent<PlayerStats>().timeState))
                    {
                        TargetPosition = transform.position;
                        return;
                    }

                    trueMove = true;

                    clip = gameObject.GetComponent<PlayerSoundController>().walk;
                    sound.clip = clip;
                    sound.Play();

                    PositionBeforeMovementStarts = transform.position;
                    moving.Current = true;

                    // gaining energy for movement
                    if (Camera.transform.parent.GetComponent<PlayerStats>().timeState == TimeState.Original)
                    {
                        movingCounter++;
                        Debug.Log("squares moved " + movingCounter.ToString());
                    }

                    // Occupy TargetPosition
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(TargetPosition, true, gameObject.GetComponentInParent<PlayerStats>().timeState);
                    //print("set occupation on " + TargetPosition.x + ", " + TargetPosition.z + " in " + gameObject.GetComponentInParent<PlayerStats>().timeState);

                    movingDirection.Current = movingDirection.Backward;
                    if (SceneManager.GetActiveScene().name == "SampleScene 1")
                    {
                        DynamicLIght(movingDirection.Current);
                    }
                    //transform.position += new Vector3(ForwardX, 0f, ForwardZ);
                }
            }
            // Strafe left pressed
            if (Input.GetKey(keyBinds.strafeLeft) && !StopMovementIfNoEnergy())
            {
                //KeyStrafeLeftPressed = false;
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, transform.right * -1, out hit, 8, 3))
                {
                    TargetPosition = transform.position + Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, -1);

                    // check occupation
                    if (MazeController.GetComponent<LabirintCreation>().GetOccupation(TargetPosition, Camera.transform.parent.GetComponent<PlayerStats>().timeState))
                    {
                        TargetPosition = transform.position;
                        return;
                    }

                    trueMove = true;

                    clip = gameObject.GetComponent<PlayerSoundController>().walk;
                    sound.clip = clip;
                    sound.Play();

                    PositionBeforeMovementStarts = transform.position;
                    moving.Current = true;

                    // gaining energy for movement
                    if (Camera.transform.parent.GetComponent<PlayerStats>().timeState == TimeState.Original)
                    {
                        movingCounter++;
                        Debug.Log("squares moved " + movingCounter.ToString());
                    }

                    // Occupy TargetPosition
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(TargetPosition, true, gameObject.GetComponentInParent<PlayerStats>().timeState);
                    //print("set occupation on " + TargetPosition.x + ", " + TargetPosition.z + " in " + gameObject.GetComponentInParent<PlayerStats>().timeState);

                    movingDirection.Current = movingDirection.StrafeLeft;
                    if (SceneManager.GetActiveScene().name == "SampleScene 1")
                    {
                        DynamicLIght(movingDirection.Current);
                    }
                }
            }

            // Strafe right pressed
            if (Input.GetKey(keyBinds.strafeRigth) && !StopMovementIfNoEnergy())
            {
                //KeyStrafeRightPressed = false;
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, transform.right, out hit, 8, 3))
                {
                    TargetPosition = transform.position + Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, 1);

                    // check occupation
                    if (MazeController.GetComponent<LabirintCreation>().GetOccupation(TargetPosition, Camera.transform.parent.GetComponent<PlayerStats>().timeState))
                    {
                        TargetPosition = transform.position;
                        return;
                    }

                    trueMove = true;

                    clip = gameObject.GetComponent<PlayerSoundController>().walk;
                    sound.clip = clip;
                    sound.Play();

                    PositionBeforeMovementStarts = transform.position;
                    moving.Current = true;

                    // gaining energy for movement
                    if (Camera.transform.parent.GetComponent<PlayerStats>().timeState == TimeState.Original)
                    {
                        movingCounter++;
                        Debug.Log("squares moved " + movingCounter.ToString());
                    }

                    // Occupy TargetPosition
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(TargetPosition, true, gameObject.GetComponentInParent<PlayerStats>().timeState);
                    //print("set occupation on " + TargetPosition.x + ", " + TargetPosition.z + " in " + gameObject.GetComponentInParent<PlayerStats>().timeState);

                    movingDirection.Current = movingDirection.StrafeRight;
                    if (SceneManager.GetActiveScene().name == "SampleScene 1")
                    {
                        DynamicLIght(movingDirection.Current);
                    }
                }
            }
        }
        if (transform.eulerAngles.y == TargetRotation.eulerAngles.y)
        {
            //moving.Rotation = false;
            if (Input.GetKey(keyBinds.turnRight) && !StopMovementIfNoEnergy())
            {
                //KeyTurnRightPressed = false;
                //print(transform.right);
                //print(Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, 1));

                clip = gameObject.GetComponent<PlayerSoundController>().turn;
                sound.clip = clip;
                sound.Play();

                moving.Current = true;
                moving.Rotation = true;
                movingDirection.Current = movingDirection.TurnRight;
                if (SceneManager.GetActiveScene().name == "SampleScene 1")
                {
                    DynamicLIght(movingDirection.Current);
                }
                CurrentRotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
                TargetRotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + 90, 0));
            }
            if (Input.GetKey(keyBinds.turnLeft) && !StopMovementIfNoEnergy())
            {
                //KeyTurnLeftPressed = false;
                //print(transform.right * -1);
                //print(Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, -1));
                
                clip = gameObject.GetComponent<PlayerSoundController>().turn;
                sound.clip = clip;
                sound.Play();

                moving.Current = true;
                moving.Rotation = true;
                movingDirection.Current = movingDirection.TurnLeft;
                if (SceneManager.GetActiveScene().name == "SampleScene 1")
                {
                    DynamicLIght(movingDirection.Current);
                }
                CurrentRotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
                TargetRotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y - 90, 0));
            }
        }
        //DynamicLIght(moving.Current);
        if (moving.Current)
        {
            Utils.GetComponent<Utils>().UpdateCursor(gameObject);
        }
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, step);

        float QuaternionAngle = Quaternion.Angle(transform.rotation, TargetRotation);
        float TSpeed = RotationSpeed / QuaternionAngle;

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, TSpeed * Time.deltaTime);
    }
    void ManageLights()
    {
        Coordinates = MazeController.GetComponent<LabirintCreation>().TilesCoordinates;
        //Vector3 MyPosition = transform.position;
        //int IndexOfTile = Coordinates.IndexOf(new Vector3(0f, 0f, 10f));
        //var IndexOfTile2 = Coordinates.IndexOf(new Vector3(MyPosition.x, 0f, (float)Math.Round(MyPosition.z)));
        //print(IndexOfTile);
        //print(IndexOfTile2);
        //print(GetCurrentTile());
        //print(transform.position);

        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");
        foreach (GameObject tile in ActiveTiles)
        {
            if (tile.transform.position.x == transform.position.x && tile.transform.position.z == transform.position.z)
            {
                //print(transform.position);
                //print(tile.transform.position);
                if (tile.GetComponent<MazeTile>().Walls[0])
                {
                    tile.GetComponent<MazeTile>().WallTorches[0].SetActive(true);
                }
                if (tile.GetComponent<MazeTile>().Walls[1])
                {
                    tile.GetComponent<MazeTile>().WallTorches[1].SetActive(true);
                }
                if (tile.GetComponent<MazeTile>().Walls[2])
                {
                    tile.GetComponent<MazeTile>().WallTorches[2].SetActive(true);
                }
                if (tile.GetComponent<MazeTile>().Walls[3])
                {
                    tile.GetComponent<MazeTile>().WallTorches[3].SetActive(true);
                }
            }
            else
            {
                if (tile.GetComponent<MazeTile>().Walls[0])
                {
                    tile.GetComponent<MazeTile>().WallTorches[0].SetActive(false);
                }
                if (tile.GetComponent<MazeTile>().Walls[1])
                {
                    tile.GetComponent<MazeTile>().WallTorches[1].SetActive(false);
                }
                if (tile.GetComponent<MazeTile>().Walls[2])
                {
                    tile.GetComponent<MazeTile>().WallTorches[2].SetActive(false);
                }
                if (tile.GetComponent<MazeTile>().Walls[3])
                {
                    tile.GetComponent<MazeTile>().WallTorches[3].SetActive(false);
                }
            }
        }
    }

    void DynamicLIght(int DirectionOfMovement)
    {
        Vector3 FuturePositionLeft = new Vector3(0, 0, 0);
        Vector3 FuturePositionRight = new Vector3(0, 0, 0);
        Vector3 FixedForwardVector = new Vector3(0, 0, 0);
        Vector3 FixedLeftVector = new Vector3(0, 0, 0);
        Vector3 CurrentPositionLeft = new Vector3(0, 0, 0);
        Vector3 CurrentPositionRight = new Vector3(0, 0, 0);
        Vector3 CurrentPositionRightPlus = new Vector3(0, 0, 0);
        Vector3 CurrentPositionLeftPlus = new Vector3(0, 0, 0);
        Vector3 CurrentPosition = PositionBeforeMovementStarts;
        int LightDistanceFixed = lightDistance;
        //print("DireftionOfMovement");
        //print(DirectionOfMovement);
        //CurrentPosition = transform.position;
        //FuturePosition = CurrentPosition + FixedForwardVector;
        //print(Moving);
        if (!LightsOn)
        {
            return;
        }
        if (DirectionOfMovement == movingDirection.Forward || DirectionOfMovement == movingDirection.StrafeLeft || DirectionOfMovement == movingDirection.StrafeRight)
        {
            LightDistanceFixed = lightDistance;
            FixedForwardVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, 1);
        }
        if (DirectionOfMovement == movingDirection.Backward)
        {
            FixedForwardVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, -1);
            LightDistanceFixed = 1;
            //CurrentPosition += FixedForwardVector;
        }
        if (DirectionOfMovement == movingDirection.TurnRight)
        {
            FixedForwardVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, 1);
            FixedLeftVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, 1);
            LightDistanceFixed = lightDistance;
            //CurrentPosition += FixedForwardVector;
        }
        if (DirectionOfMovement == movingDirection.TurnLeft)
        {
            FixedForwardVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, 1);
            FixedLeftVector = Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.right, -1);
            LightDistanceFixed = lightDistance;
            //CurrentPosition += FixedForwardVector;
        }
        //if (moving.Current)
        //{
        //    MovingCorrection = 1;

        //}
        //else
        //{
        //    MovingCorrection = 0;

        //}
        if (FixedForwardVector.z == 10f)
        {
            CurrentPositionLeft = new Vector3(CurrentPosition.x - 10, 0, CurrentPosition.z);
            CurrentPositionRight = new Vector3(CurrentPosition.x + 10, 0, CurrentPosition.z);

            if (DirectionOfMovement == movingDirection.StrafeRight)
            {
                CurrentPositionRightPlus = new Vector3(CurrentPosition.x + 20, 0, CurrentPosition.z);
            }
            if (DirectionOfMovement == movingDirection.StrafeLeft)
            {
                CurrentPositionLeftPlus = new Vector3(CurrentPosition.x - 20, 0, CurrentPosition.z);
            }
        }
        if (FixedForwardVector.z == -10f)
        {
            CurrentPositionLeft = new Vector3(CurrentPosition.x + 10, 0, CurrentPosition.z);
            CurrentPositionRight = new Vector3(CurrentPosition.x - 10, 0, CurrentPosition.z);

            if (DirectionOfMovement == movingDirection.StrafeRight)
            {
                CurrentPositionRightPlus = new Vector3(CurrentPosition.x - 20, 0, CurrentPosition.z);
            }
            if (DirectionOfMovement == movingDirection.StrafeLeft)
            {
                CurrentPositionLeftPlus = new Vector3(CurrentPosition.x + 20, 0, CurrentPosition.z);
            }
        }
        if (FixedForwardVector.x == 10f)
        {
            CurrentPositionLeft = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 10);
            CurrentPositionRight = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 10);
            if (DirectionOfMovement == movingDirection.StrafeRight)
            {
                CurrentPositionRightPlus = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 20);
            }
            if (DirectionOfMovement == movingDirection.StrafeLeft)
            {
                CurrentPositionLeftPlus = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 20);
            }
        }
        if (FixedForwardVector.x == -10f)
        {
            CurrentPositionLeft = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 10);
            CurrentPositionRight = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 10);
            if (DirectionOfMovement == movingDirection.StrafeRight)
            {
                CurrentPositionRightPlus = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 20);
            }
            if (DirectionOfMovement == movingDirection.StrafeLeft)
            {
                CurrentPositionLeftPlus = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 20);
            }
        }

        if (FixedLeftVector.z == 10f)
        {
            FuturePositionLeft = new Vector3(CurrentPosition.x - 10, 0, CurrentPosition.z);
            FuturePositionRight = new Vector3(CurrentPosition.x + 10, 0, CurrentPosition.z);
        }
        if (FixedLeftVector.z == -10f)
        {
            FuturePositionLeft = new Vector3(CurrentPosition.x + 10, 0, CurrentPosition.z);
            FuturePositionRight = new Vector3(CurrentPosition.x - 10, 0, CurrentPosition.z);
        }
        if (FixedLeftVector.x == 10f)
        {
            FuturePositionLeft = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 10);
            FuturePositionRight = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 10);
        }
        if (FixedLeftVector.x == -10f)
        {
            FuturePositionLeft = new Vector3(CurrentPosition.x, 0, CurrentPosition.z - 10);
            FuturePositionRight = new Vector3(CurrentPosition.x, 0, CurrentPosition.z + 10);
        }
        //print(FuturePositionLeft);
        //print(FuturePositionRight);
        PositionsToTurnLight.Clear();
        //CurrentPositionForward.Clear();
        //CurrentPositionLeftForward.Clear();
        //CurrentPositionRightForward.Clear();
        //CurrentPositionForward.Add(CurrentPosition);
        PositionsToTurnLight.Add(CurrentPosition);

        Vector3 TempPosition = CurrentPosition;
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedForwardVector.x, 0f, FixedForwardVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        TempPosition = CurrentPositionLeft;
        PositionsToTurnLight.Add(CurrentPositionLeft);
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedForwardVector.x, 0f, FixedForwardVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        TempPosition = CurrentPositionRight;
        PositionsToTurnLight.Add(CurrentPositionRight);
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedForwardVector.x, 0f, FixedForwardVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        // Add lightened squares if strafed right
        if (DirectionOfMovement == movingDirection.StrafeRight)
        {
            TempPosition = CurrentPositionRightPlus;
            PositionsToTurnLight.Add(CurrentPositionRightPlus);
            for (int i = 0; i < LightDistanceFixed; ++i)
            {
                TempPosition += new Vector3(FixedForwardVector.x, 0f, FixedForwardVector.z);
                PositionsToTurnLight.Add(TempPosition);
            }
        }

        // Add lightened squares if strafed left
        if (DirectionOfMovement == movingDirection.StrafeLeft)
        {
            TempPosition = CurrentPositionLeftPlus;
            PositionsToTurnLight.Add(CurrentPositionLeftPlus);
            for (int i = 0; i < LightDistanceFixed; ++i)
            {
                TempPosition += new Vector3(FixedForwardVector.x, 0f, FixedForwardVector.z);
                PositionsToTurnLight.Add(TempPosition);
            }
        }



        TempPosition = CurrentPosition;
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedLeftVector.x, 0f, FixedLeftVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        TempPosition = FuturePositionLeft;
        PositionsToTurnLight.Add(FuturePositionLeft);
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedLeftVector.x, 0f, FixedLeftVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        TempPosition = FuturePositionRight;
        PositionsToTurnLight.Add(FuturePositionRight);
        for (int i = 0; i < LightDistanceFixed; i++)
        {
            TempPosition += new Vector3(FixedLeftVector.x, 0f, FixedLeftVector.z);
            PositionsToTurnLight.Add(TempPosition);
        }

        //CurrentPositionRightForward.Add(CurrentPositionRight);
        //for (int i = 1; i < lightDistance + MovingCorrection; i++)
        //{
        //    CurrentPositionRight += new Vector3(ForwardX, 0f, ForwardZ);
        //    CurrentPositionRightForward.Add(CurrentPositionRight);
        //}

        ActiveTiles = GameObject.FindGameObjectsWithTag("MazeTile");

        //foreach (var Position in CurrentPositionForward)
        //{
        //    int IndexOfTile = MazeController.GetComponent<MazeControllerV2>().TilesCoordinates.IndexOf(Position);
        //    if (IndexOfTile > 0)
        //    {
        //        var VarTile = MazeController.GetComponent<MazeControllerV2>().Tiles[IndexOfTile];
        //        //var TorchBoolId = 0;
        //        foreach (var item in VarTile.GetComponent<MazeTile>().WallsObjects)
        //        {
        //            if (item.GetComponent<WallStuff>().Torch && !item.GetComponent<WallStuff>().TorchObject.activeSelf)
        //            {
        //                item.GetComponent<WallStuff>().TorchObject.SetActive(true);

        //            }
        //        }
        //    }
        //}
        //foreach (var Position in CurrentPositionLeftForward)
        //{
        //    int IndexOfTile = MazeController.GetComponent<MazeControllerV2>().TilesCoordinates.IndexOf(Position);
        //    if (IndexOfTile > 0)
        //    {
        //        var VarTile = MazeController.GetComponent<MazeControllerV2>().Tiles[IndexOfTile];
        //        foreach (var item in VarTile.GetComponent<MazeTile>().WallsObjects)
        //        {
        //            if (item.GetComponent<WallStuff>().Torch)
        //            {
        //                item.GetComponent<WallStuff>().TorchObject.SetActive(true);

        //            }
        //        }
        //    }
        //}
        //print(PositionsToTurnLight);
        foreach (var Position in PositionsToTurnLight)
        {
            int IndexOfTile = MazeController.GetComponent<LabirintCreation>().TilesCoordinates.IndexOf(Position);
            if (IndexOfTile > 0)
            {
                var VarTile = MazeController.GetComponent<LabirintCreation>().Tiles[IndexOfTile];
                foreach (var item in VarTile.GetComponent<MazeTile>().WallsObjects)
                {
                    if (item.GetComponent<WallStuff>().Torch)
                    {
                        item.GetComponent<WallStuff>().TorchObject.SetActive(true);

                    }
                }
            }
        }

        foreach (var tile in ActiveTiles)
        {
            int IndexOfTile = PositionsToTurnLight.IndexOf(tile.transform.position);
            if (IndexOfTile == -1)
            {
                foreach (var item in tile.GetComponent<MazeTile>().WallsObjects)
                {
                    if (item.GetComponent<WallStuff>().Torch && !moving.Current)
                    {
                        item.GetComponent<WallStuff>().TorchObject.SetActive(false);
                    }
                }
                //foreach (var item in tile.GetComponent<MazeTile>().WallTorches)
                //{
                //    if (item.activeSelf && !moving.Current)
                //    {
                //        item.SetActive(false);
                //    }
                //}
            }
        }
    }

    private bool StopMovementIfNoEnergy()
    {
        // Stop moving if no energy
        if (gameObject.GetComponentInParent<PlayerStats>().timeState == TimeState.Shifted && gameObject.GetComponentInParent<PlayerStats>().energy <= 0)
        {

            //print("state is " + gameObject.GetComponentInParent<PlayerStats>().timeState.ToString());
            //print("energy is " + gameObject.GetComponentInParent<PlayerStats>().energy.ToString());
            return true;
        }
        return false;
    }

    /*private List<GameObject> FindMonstersNearby(Transform playerPosition, int range)
    {
        List<GameObject> monsters = new List<GameObject>();
        for (int x = -1 * range; x <= range; ++x)
        {
            for (int z = -1 * range; z <= range; ++z)
            {
                if (x != 0 && z != 0)
                {
                    float targetX = playerPosition.position.x + squareSize * x;
                    float targetZ = playerPosition.position.z + squareSize * z;

                    if (playerPosition.position.x == targetX && playerPosition.position.z == targetZ)
                    {
                        //return true;
                    }
                }
            }
        }
        return false;
    }*/

    //private Vector3 Utils.GetComponent<Utils>().GetFixedDirectionVector(Vector3 DirectionVector, int direction)
    //{
    //    float ForwardX = 0f;
    //    float ForwardZ = 0f;
    //    if (DirectionVector.x * direction < -0.5)
    //    {
    //        ForwardX = -10f;
    //    }
    //    else if (DirectionVector.x * direction > 0.5)
    //    {
    //        ForwardX = 10f;
    //    }
    //    else
    //    {
    //        ForwardX = 0f;
    //    }
    //    if (DirectionVector.z * direction < -0.5)
    //    {
    //        ForwardZ = -10f;
    //    }
    //    else if (DirectionVector.z * direction > 0.5)
    //    {
    //        ForwardZ = 10f;
    //    }
    //    else
    //    {
    //        ForwardZ = 0f;
    //    }
    //    return new Vector3(ForwardX, 0f, ForwardZ);
    //}

    public class Moving
    {
        public bool Started;
        public bool Finished;
        public bool Current;
        public bool Rotation;

        public Moving()
        {
            Started = true;
            Finished = false;
        }
    }

    private class MovingDirection
    {
        public int Forward;
        public int Backward;
        public int StrafeLeft;
        public int StrafeRight;
        public int TurnLeft;
        public int TurnRight;
        public int Current;

        public MovingDirection()
        {
            Forward = 0;
            Backward = 1;
            StrafeLeft = 2;
            StrafeRight = 3;
            TurnLeft = 4;
            TurnRight = 5;
        }
    }
}
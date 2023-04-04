using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{
    public Texture2D CursorDefault;
    public Texture2D CursorAttack;
    public Texture2D CursorBreak;
    public GameObject Player;
    public bool PlayerMoving;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetFixedDirectionVector(Vector3 DirectionVector, int direction)
    {
        float ForwardX = 0f;
        float ForwardZ = 0f;
        if (DirectionVector.x * direction < -0.5)
        {
            ForwardX = -10f;
        }
        else if (DirectionVector.x * direction > 0.5)
        {
            ForwardX = 10f;
        }
        else
        {
            ForwardX = 0f;
        }
        if (DirectionVector.z * direction < -0.5)
        {
            ForwardZ = -10f;
        }
        else if (DirectionVector.z * direction > 0.5)
        {
            ForwardZ = 10f;
        }
        else
        {
            ForwardZ = 0f;
        }
        return new Vector3(ForwardX, 0f, ForwardZ);
    }

    public void UpdateCursor(GameObject Target, CursorAction Action = CursorAction.Default)
    {
        if (Action == CursorAction.Default)
        {
            Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.Auto);
        }
        if (FacingGameObject(Target))
        {
            switch (Action)
            {
                case CursorAction.Attack:
                    Cursor.SetCursor(CursorAttack, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorAction.Break:
                    Cursor.SetCursor(CursorBreak, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }

    public bool FacingGameObject(GameObject Target)
    {
        RaycastHit Hit;
        if (Physics.Raycast(Player.transform.position, Player.transform.forward, out Hit, 8f, 3)) //vozmozhno nado dorabotat, tut mi tozhe ignoriruem meshki s lutom
        {
            if (Hit.transform.gameObject == Target)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}

public class WallSides
{
    public int North;
    public int South;
    public int West;
    public int East;

    // ������� ������ �� �������
    public WallSides()
    {
        North = 0;
        South = 1;
        West = 2;
        East = 3;
    }
}

public class KeyBinds
{
    public KeyCode moveForward;
    public KeyCode moveBackward;
    public KeyCode turnLeft;
    public KeyCode turnRight;
    public KeyCode strafeLeft;
    public KeyCode strafeRigth;
    public KeyCode LeftMouseClick;

    // ������� ������ �� �������
    public KeyBinds()
    {
        moveForward = KeyCode.W;
        moveBackward = KeyCode.S;
        turnLeft = KeyCode.A;
        turnRight = KeyCode.D;
        strafeLeft = KeyCode.Q;
        strafeRigth = KeyCode.E;
        LeftMouseClick = KeyCode.Mouse0;
    }
}

//public class ObjectTypes
//{
//    public int Wall;
//    public int Monster;

//    // ������� ������ �� �������
//    public ObjectTypes()
//    {
//        Wall = 0;
//        Monster = 1;
//    }
//}

public enum ObjectTypes
{
    Wall = 0,
    Monster = 1,
    Redpill = 2,
    Bluepill = 3,
    Thirdpill = 4,
    LootSack = 5,
    Chest = 6
}

public enum CursorAction
{
    Default = 0,
    Attack = 1,
    Break = 2,
    Lockpick = 3,
    Use = 4
}

public enum TimeState
{
    Original = 0,
    Shifted = 1
}

public interface Hittable
{
    public ObjectTypes HittableObjectType { get; }
    public bool Hit();
    public void OnMouseEnter();
    public void OnMouseExit();
    public void OnMouseOver();
}

public interface Shiftable
{
    public TimeState timeState {get; set;}

    public void Shift();
}
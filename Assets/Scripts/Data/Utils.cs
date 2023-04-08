using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Utils : MonoBehaviour
{
    public Texture2D CursorDefault;
    public Texture2D CursorAttack;
    public Texture2D CursorBreak;
    public Texture2D CursorUse;
    public Texture2D CursorAttackPressed;
    public Texture2D CursorBreakPressed;
    public Texture2D CursorUsePressed;
    public GameObject Player;
    public bool PlayerMoving;

    public List<LootItem> lootBox;
    // Start is called before the first frame update
    void Start()
    {
        lootBox = new List<LootItem>();
        Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.ForceSoftware);

        // Filling the LootBox
        // health pot 
        for (int i = 0; i < 9; ++i)
        {
            LootItem item = new HealthPotion(Player);
            lootBox.Add(item);
        }

        // energu booster
        for (int i = 0; i < 9; ++i)
        {
            LootItem item = new EnergyBoost(Player);
            lootBox.Add(item);
        }

        // speed potion
        for (int i = 0; i < 4; ++i)
        {
            LootItem item = new SpeedPotion(Player);
            lootBox.Add(item);
        }

        // agility pot
        for (int i = 0; i < 4; ++i)
        {
            LootItem item = new AgilityPotion(Player);
            lootBox.Add(item);
        }

        // torch orig
        for (int i = 0; i < 4; ++i)
        {
            LootItem item = new Torch(Player, TimeState.Original);
            lootBox.Add(item);
        }

        // torch shifted
        for (int i = 0; i < 2; ++i)
        {
            LootItem item = new Torch(Player, TimeState.Shifted);
            lootBox.Add(item);
        }

        // dexterity pot
        for (int i = 0; i < 2; ++i)
        {
            LootItem item = new DexterityPotion(Player);
            lootBox.Add(item);
        }

        // shifter cooler
        for (int i = 0; i < 4; ++i)
        {
            LootItem item = new ShifterCooler(Player);
            lootBox.Add(item);
        }

        // vitality pot
        for (int i = 0; i < 2; ++i)
        {
            LootItem item = new VitalityPotion(Player);
            lootBox.Add(item);
        }

        // energy band
        for (int i = 0; i < 2; ++i)
        {
            LootItem item = new EnergyBank(Player);
            lootBox.Add(item);
        }

        // shifter core
        for (int i = 0; i < 3; ++i)
        {
            LootItem item = new ShifterCore(Player);
            lootBox.Add(item);
        }

        // shifter charger
        for (int i = 0; i < 3; ++i)
        {
            LootItem item = new ShifterCharger(Player);
            lootBox.Add(item);
        }

        // strength pot
        for (int i = 0; i < 2; ++i)
        {
            LootItem item = new StrengthPotion(Player);
            lootBox.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetLoot(TimeState state)
    {
        IEnumerable<LootItem> items = lootBox.Where(item => item.existenceState == state);
        int max = items.Count();

        int lootIndex = new System.Random().Next(0, max - 1);
        int index = 0;

        foreach (LootItem item in items)
        {
            if (index == lootIndex)
            {
                item.GetBoon();
                lootBox.Remove(item);
                //Player.GetComponent<PlayerLogController>().Message(lootBox.Count() + " left");
                return;
            }
            ++index;
        }

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
            Cursor.SetCursor(CursorDefault, Vector2.zero, CursorMode.ForceSoftware);
        }
        if (FacingGameObject(Target))
        {
            switch (Action)
            {
                case CursorAction.Attack:
                    Cursor.SetCursor(CursorAttack, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorAction.Break:
                    Cursor.SetCursor(CursorBreak, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorAction.Use:
                    Cursor.SetCursor(CursorUse, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorAction.AttackPressed:
                    Cursor.SetCursor(CursorAttackPressed, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorAction.BreakPressed:
                    Cursor.SetCursor(CursorBreakPressed, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorAction.UsePressed:
                    Cursor.SetCursor(CursorUsePressed, Vector2.zero, CursorMode.ForceSoftware);
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

    public abstract class LootItem
    {
        public GameObject player;
        public TimeState existenceState;

        //public LootItem(GameObject player, List<TimeState> states)
        public LootItem(GameObject player)
        {
            this.player = player;
            /*foreach (TimeState state in states)
            {
                existenceStates.Add(state);
            }*/
        }
        public abstract void GetBoon();
    }

    public class HealthPotion : LootItem
    {
        public HealthPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().TakeDamage(-3);
            player.GetComponent<PlayerLogController>().Message("You've found a healing potion! Your health increased.");
            print("You've found a healing potion! Your health increased.");
        }
    }

    public class EnergyBoost : LootItem
    {
        public EnergyBoost(GameObject player) : base(player)
        {
            existenceState = TimeState.Original;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().ChangeEnergy(3);
            player.GetComponent<PlayerLogController>().Message("You've found an energy boost! Your energy increased.");
            print("You've found an energy boost! Your energy increased.");
        }
    }

    public class SpeedPotion : LootItem
    {
        public SpeedPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerMovement>().Speed += 2;
            player.GetComponent<PlayerLogController>().Message("You've found a speed potion! You became faster.");
            print("You've found a speed potion! You became faster.");
        }
    }

    public class AgilityPotion : LootItem
    {
        public AgilityPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerMovement>().RotationSpeed += 20;
            player.GetComponent<PlayerLogController>().Message("You've found an agility potion! Your turns became faster.");
            print("You've found an agility potion! Your turns became faster.");
        }
    }

    public class Torch : LootItem
    {
        public Torch(GameObject player, TimeState state) : base(player)
        {
            existenceState = state;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerMovement>().Torch.GetComponent<Light>().intensity += 5;
            player.GetComponent<PlayerMovement>().Torch.GetComponent<Light>().range += 5;
            player.GetComponent<PlayerLogController>().Message("You've found a torch! It radiates some more light.");
            print("You've found a torch! It radiates some more light.");
        }
    }

    public class DexterityPotion : LootItem
    {
        public DexterityPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().attackSpeed -= 1;
            player.GetComponent<PlayerLogController>().Message("You've found a dexterity potion! Your attacks became faster.");
            print("You've found a dexterity potion! Your attacks became faster.");
        }
    }

    public class ShifterCooler : LootItem
    {
        public ShifterCooler(GameObject player) : base(player)
        {
            existenceState = TimeState.Original;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().shiftSpeed -= 1;
            player.GetComponent<PlayerLogController>().Message("You've found a shifter cooler! Now it cools faster.");
            print("You've found a shifter cooler! Now it cools faster.");
        }
    }

    public class VitalityPotion : LootItem
    {
        public VitalityPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().maxHealth += 25;
            player.GetComponent<PlayerStats>().health += 25;
            player.GetComponent<PlayerLogController>().Message("You've found a vitality potion! Your maximum health increased.");
            print("You've found a vitality potion! Your maximum health increased.");
        }
    }

    public class EnergyBank : LootItem
    {
        public EnergyBank(GameObject player) : base(player)
        {
            existenceState = TimeState.Original;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().maxEnergy += 25;
            player.GetComponent<PlayerStats>().energy += 25;
            player.GetComponent<PlayerLogController>().Message("You've found an energy bank! Your maximum energy increased.");
            print("You've found an energy bank! Your maximum energy increased.");
        }
    }

    public class ShifterCore : LootItem
    {
        public ShifterCore(GameObject player) : base(player)
        {
            existenceState = TimeState.Original;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().energyLoseRate += 1;
            player.GetComponent<PlayerLogController>().Message("You've found a shifter core! You lose your energy slower.");
            print("You've found a shifter core! You lose your energy slower.");
        }
    }

    public class ShifterCharger : LootItem
    {
        public ShifterCharger(GameObject player) : base(player)
        {
            existenceState = TimeState.Original;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().restoreEnergyCounter -= 2;
            player.GetComponent<PlayerLogController>().Message("You've found a shifter charger! Your energy restores more quickly.");
            print("You've found a shifter charger! Your energy restores more quickly.");
        }
    }

    public class StrengthPotion : LootItem
    {
        public StrengthPotion(GameObject player) : base(player)
        {
            existenceState = TimeState.Shifted;
        }

        public override void GetBoon()
        {
            player.GetComponent<PlayerStats>().damage += 1;
            player.GetComponent<PlayerLogController>().Message("You've found a strength potion! Your strikes become more deadly.");
            print("You've found a strength potion! Your strikes become more deadly.");
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
    Chest = 6,
    ButtonOld = 7,
    ButtonFuture = 8,
    Portal = 9
}

public enum CursorAction
{
    Default = 0,
    Attack = 1,
    Break = 2,
    Lockpick = 3,
    Use = 4,
    AttackPressed = 5,
    BreakPressed = 6,
    UsePressed = 7
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

/*public interface Shiftable
{
    public TimeState timeState {get; set;}

    public void Shift();
}*/

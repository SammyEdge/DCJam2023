//using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, Hittable//, Shiftable
{
    private GameObject Utils;
    private GameObject Player;
    private GameObject GameState;
    private GameObject MazeController;
    public GameObject LootSack;
    private float timer = 2;

    public AudioSource sound;
    private List<Vector3> path;

    private static int squareSize = 10;

    // radius to hear player and start turning to him
    private int perceptionRadius = 2;

    //Visual distance to see player
    private int visionDistance = 4;

    private int attackDistance = 1;
    private int Speed = 10;
    public ObjectTypes HittableObjectType = ObjectTypes.Monster;

    private int ignoreLayer = 3;

    // ref to target position
    public Vector3 playerPosition;
    public Vector3 knownPlayersLocation, destination;
    public Vector3 startPosition;

    //states
    public bool isWaiting = true, isChasing = false, isAttacking = false, isMoving = false, isChecking = false;


    ObjectTypes Hittable.HittableObjectType { get => this.HittableObjectType; }
    //public TimeState timeState { get => this.timeState; set => this.timeState = value; }
    public TimeState timeState;
    //public ObjectTypes HittableObjectType = ObjectTypes.Wall;
    // Start is called before the first frame update
    void Start()
    {
        //print(HittableObjectType);
        Utils = GameObject.FindGameObjectWithTag("Utils");
        Player = GameObject.FindGameObjectWithTag("Player");
        GameState = GameObject.FindGameObjectWithTag("GameState");
        MazeController = GameObject.FindGameObjectWithTag("MazeController");

        //timeState = TimeState.Original;

        playerPosition = Player.GetComponent<PlayerMovement>().TargetPosition;

        sound = gameObject.transform.GetComponent<AudioSource>();

        startPosition = gameObject.transform.position;
        destination = gameObject.transform.position;

        // чтобы инстанцировалось
        knownPlayersLocation = gameObject.transform.position;
        //print(Utils.GetComponent<Utils>().GetFixedDirectionVector(transform.forward, -1));
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = Player.GetComponent<PlayerMovement>().TargetPosition;
        timer -= Time.deltaTime;

        if (isMoving)
        {
            float step = Speed * Time.deltaTime;
            //AudioClip ac = gameObject.GetComponent<MonsterSoundController>().walk;
            //print("audio " + ac.name);
            //sound.clip = ac;
            //sound.Play();

            //print(gameObject.name + ": moving at " + step.ToString() + " speed");

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, step);

            if (gameObject.transform.position.Equals(destination))
            {
                isMoving = false;
                isWaiting = true;
                isAttacking = false;
                isChasing = false;
                isChecking = false;

                // unoccupy square
                MazeController.GetComponent<LabirintCreation>().SetOccupation(startPosition, false, timeState);

                startPosition = gameObject.transform.position;
                gameObject.transform.LookAt(playerPosition);
                sound.Stop();
            }
        }


        if (timer < 0)
        {
            // knows about player's last location and not there
            if (isChecking)
            {
                // check occupation
                if (MazeController.GetComponent<LabirintCreation>().GetOccupation(knownPlayersLocation, timeState))
                {
                    return;
                }


                destination = knownPlayersLocation;
                isMoving = true;

                MazeController.GetComponent<LabirintCreation>().SetOccupation(destination, true, timeState);
                startPosition = gameObject.transform.position;

                AudioClip ac = gameObject.GetComponent<MonsterSoundController>().walk;
                sound.clip = ac;
                sound.Play();

                isChasing = false;
                isAttacking = false;
                isChecking = false;
                print(gameObject.name + ": trying to find him");
                return;
            }

            // attack player
            if (isAttacking)
            {
                //проверка возможности удара
                RaycastHit monsterLookHit;
                // хватает дистанции и видит
                if (Physics.Raycast(gameObject.transform.position, playerPosition - gameObject.transform.position, out monsterLookHit, attackDistance * squareSize, ignoreLayer))
                {
                    Transform checkedTransform = monsterLookHit.transform;
                    if (checkedTransform.parent != null)
                    {
                        checkedTransform = checkedTransform.parent.transform;
                    }

                    if (checkedTransform == Player.transform)
                    {
                        // strike
                        PlayAttackAnimation();
                        if (Player.GetComponent<PlayerStats>().timeState == TimeState.Original)
                        {
                            Player.GetComponent<PlayerStats>().TakeDamage(1);
                        }
                        else
                        {
                            Player.GetComponent<PlayerStats>().ChangeEnergy(-1);
                        }
                        print(gameObject.name + ": attacking player");
                        knownPlayersLocation = playerPosition;
                        Debug.DrawRay(gameObject.transform.position, playerPosition - gameObject.transform.position, Color.green, 2);
                        timer = 2;
                        return;
                    }
                    else
                    {
                        isAttacking = false;
                        Debug.DrawRay(gameObject.transform.position, knownPlayersLocation - gameObject.transform.position, Color.yellow, 2);
                        isChecking = true;
                        timer = 2;
                        print(gameObject.name + ": he is gone!");
                        return;
                    }
                }
                else
                {
                    isAttacking = false;
                    Debug.DrawRay(gameObject.transform.position, knownPlayersLocation - gameObject.transform.position, Color.blue, 2);
                    isChecking = true;
                    timer = 2;
                    print(gameObject.name + ": he is missed!");
                    return;
                }
            }

            /*while (Player.GetComponent<PlayerMovement>().moving.Current)
            {
                // waiting for movement end
                print(gameObject.name + ": waiting to end movement");
            }
            */
            //print("action");
            //PlayAttackAnimation();

            // rotate to player
            //if (isWaiting)
            // {
            //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, gameObject.transform.eulerAngles.y - 90, 0));
            //}
            //else
            //{
            // Check vision to player and turn to him
            if (FindPlayer(gameObject.transform.position, Player.GetComponent<PlayerMovement>().TargetPosition, perceptionRadius))
            {
                print(gameObject.name + ": I hear a player");
                gameObject.transform.LookAt(playerPosition);

                RaycastHit monsterLookHit;
                if (Physics.Raycast(gameObject.transform.position, playerPosition - gameObject.transform.position, out monsterLookHit, visionDistance * squareSize, ignoreLayer))
                {

                    Transform checkedTransform = monsterLookHit.transform;
                    if (checkedTransform.parent != null)
                    {
                        checkedTransform = checkedTransform.parent.transform;
                    }

                    if (checkedTransform != Player.transform)
                    {
                        print(gameObject.name + ": I see towards a player, but see " + monsterLookHit.transform.name);
                        isWaiting = true;
                        isAttacking = false;
                        isChasing = false;
                        isMoving = false;
                    }
                    else
                    {
                        print(gameObject.name + ": I see you, creepy player!");
                        isChasing = true;
                        isWaiting = false;
                        isAttacking = false;
                        isMoving = false;
                    }

                    Debug.DrawRay(gameObject.transform.position, playerPosition - gameObject.transform.position, Color.red, 2);
                    //print(Player.transform.position.x.ToString() + " " + Player.transform.position.z.ToString());
                }
            }
            else
            {
                if (isWaiting)
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, gameObject.transform.eulerAngles.y - 90, 0));
                }
            }

            // Chase player to attack him
            if (isChasing)
            {
                // If range is enough to attack, then attack
                RaycastHit monsterLookHit;
                if (Physics.Raycast(gameObject.transform.position, playerPosition - gameObject.transform.position, out monsterLookHit, attackDistance * squareSize, ignoreLayer))
                {
                    if (monsterLookHit.transform.parent.transform == Player.transform)
                    {
                        gameObject.transform.LookAt(Player.transform);
                        isAttacking = true;
                        isWaiting = false;
                        isChasing = false;
                        isMoving = false;
                        timer = 0;
                        return;
                    }
                }
                else
                {
                    isAttacking = false;
                    // Find closest positions to attack around player: forward, backward, left and right and go there
                    //List<Vector3> neighbours = GetNeighbours(playerPosition, squareSize);

                    // New algorythm
                    // Shoot vector to player, find smallest angle between vector and axis, move to that axis on 1 square if accessible, else other side, then repeat

                    Vector3 target;

                    //Vector3 movingVector = playerPosition - gameObject.transform.position;
                    Vector3 zMovingVector = new Vector3(gameObject.transform.position.x, 0, playerPosition.z);// - gameObject.transform.position;
                    Vector3 xMovingVector = new Vector3(playerPosition.x, 0, gameObject.transform.position.z);// - gameObject.transform.position;

                    // Check nearest squares
                    //Vector3 xNearestSquare = new Vector3(xMovingVector.x, 0, xMovingVector.z - xMovingVector.z / Math.Abs(xMovingVector.z) * squareSize);
                    float deltaX = 0, deltaZ = 0;

                    // zero division check
                    if (xMovingVector.x - gameObject.transform.position.x != 0)
                    {
                        deltaX = gameObject.transform.position.x + (xMovingVector.x - gameObject.transform.position.x) / Math.Abs(xMovingVector.x - gameObject.transform.position.x) * squareSize;
                    }
                    else
                    {
                        deltaX = gameObject.transform.position.x;
                    }

                    if (zMovingVector.z - gameObject.transform.position.z != 0)
                    {
                        deltaZ = gameObject.transform.position.z + (zMovingVector.z - gameObject.transform.position.z) / Math.Abs(zMovingVector.z - gameObject.transform.position.z) * squareSize;
                    }
                    else
                    {
                        deltaZ = gameObject.transform.position.z;
                    }

                    Vector3 xNearestSquare = new Vector3(deltaX, 0, xMovingVector.z);
                    print(gameObject.name + ": my nearest x square: " + xNearestSquare.x + " " + xNearestSquare.z);

                    Vector3 zNearestSquare = new Vector3(zMovingVector.x, 0, deltaZ);
                    print(gameObject.name + ": my nearest z square: " + zNearestSquare.x + " " + zNearestSquare.z);

                    bool xOk = false, zOk = false;

                    if (!Physics.Raycast(gameObject.transform.position, xNearestSquare - gameObject.transform.position, squareSize, ignoreLayer))
                    {
                        xOk = true;
                    }

                    if (!Physics.Raycast(gameObject.transform.position, zNearestSquare - gameObject.transform.position, squareSize, ignoreLayer))
                    {
                        zOk = true;
                    }

                    if (xOk && !zOk)
                    {
                        // x square available and z square unavailable
                        target = xNearestSquare;
                        print(gameObject.name + ": moving x: " + target.x + " " + target.z);
                    }
                    else if (!xOk && zOk)
                    {
                        // x square unavailable and z square available 
                        target = zNearestSquare;
                        print(gameObject.name + ": moving z" + target.x + " " + target.z);
                    }
                    else if (!xOk && !zOk)
                    {
                        // both not available
                        isWaiting = true;
                        isChasing = false;
                        timer = 2;
                        print(gameObject.name + ": no nearest squares to move");
                        return;
                    }
                    else
                    {
                        // both available
                        float xMovingVectorLength = (xMovingVector - gameObject.transform.position).sqrMagnitude;
                        float zMovingVectorLength = (zMovingVector - gameObject.transform.position).sqrMagnitude;

                        // moving towards xvector
                        if (xMovingVectorLength > zMovingVectorLength)
                        {
                            target = xNearestSquare;
                            print(gameObject.name + ": moving X" + target.x + " " + target.z);
                        }
                        else if (xMovingVectorLength < zMovingVectorLength)
                        {
                            target = zNearestSquare;
                            print(gameObject.name + ": moving Z" + target.x + " " + target.z);
                        }
                        else
                        {
                            int rand = new System.Random().Next(0, 1);
                            if (rand == 0)
                            {
                                target = xNearestSquare;
                                print(gameObject.name + ": decide to move x" + target.x + " " + target.z);
                            }
                            else
                            {
                                target = zNearestSquare;
                                print(gameObject.name + ": decide to move z" + target.x + " " + target.z);
                            }
                        }
                    }

                    // Check occupation
                    if (MazeController.GetComponent<LabirintCreation>().GetOccupation(target, timeState))
                    {
                        return;
                    }


                    startPosition = gameObject.transform.position;
                    destination = target;

                    // set occupation
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(destination, true, timeState);
                    // unset occupation
                    //MazeController.GetComponent<LabirintCreation>().SetOccupation(gameObject.transform.position, false);

                    isMoving = true;
                    AudioClip ac = gameObject.GetComponent<MonsterSoundController>().walk;
                    sound.clip = ac;
                    sound.Play();
                    isChasing = false;
                    timer = 0;
                    return;

                    /*
                    // find closest square to move
                    float lessMagnitude;
                    Vector3 target;

                    // Проверить соседние клетки с игроком и их досягаемость
                    if (neighbours.Count > 0)
                    {
                        print(gameObject.name + ": spare squares to move " + neighbours.Count);
                        lessMagnitude = (neighbours[0] - gameObject.transform.position).sqrMagnitude;
                        target = neighbours[0];

                        for (int i = 1; i < neighbours.Count; ++i)
                        {
                            Vector3 check = neighbours[i];
                            float checkMagnitude = (check - gameObject.transform.position).sqrMagnitude;
                            if (checkMagnitude < lessMagnitude)
                            {
                                target = check;
                                lessMagnitude = checkMagnitude;
                            }
                        }
                        print(gameObject.name + ": closest square to player is " + target.x + " " + target.z);

                        // going there
                        isMoving = true;
                        destination = target;
                        isChasing = false;
                        // then move

                        //path = SearchInWidth(gameObject.transform.position, target);

                        //print ("squares in path " + path.Count);
                        //if (path != null && path.Count > 0)
                        //{

                        // взять следующую клетку и идти туда
                        //    for (int i = 1; i < path.Count; ++i)
                        //    {
                        //var step = Speed * Time.deltaTime;
                        //gameObject.transform.position = Vector3.MoveTowards(transform.position, path[i], step);
                        //    }
                        //}

                        // Если не нашли, искать другой путь.
                        // Но это и так сработает на другом тике
                    }
                    else
                    {
                        isWaiting = true;
                    }
                    */
                }
            }

            timer = 2;
        }
    }

    private bool FindPlayer(Vector3 monsterPosition, Vector3 playerPosition, int range)
    {
        // does not working with diagonals
        /*if ((playerPosition.position - monsterPosition.position).magnitude <= range * squareSize)
        {
            return true;
        }
        return false;*/

        for (int x = -1 * range; x <= range; ++x)
        {
            for (int z = -1 * range; z <= range; ++z)
            {
                float targetX = monsterPosition.x + squareSize * x;
                float targetZ = monsterPosition.z + squareSize * z;

                if (playerPosition.x == targetX && playerPosition.z == targetZ)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static List<Vector3> SearchInWidth(Vector3 entry, Vector3 target)
    {
        Dictionary<int, Vector3> visited = new Dictionary<int, Vector3>();
        Queue<Vector3> toVisit = new Queue<Vector3>();

        //key: square we stand, value: where we came from
        Dictionary<Vector3, Vector3> map = new Dictionary<Vector3, Vector3>();


        bool found = false;

        float magnitude = 0;

        magnitude = (target - entry).magnitude;

        // Кладём входную клетку
        toVisit.Enqueue(entry);

        map.Add(entry, entry);

        while (toVisit.Count > 0)
        {
            // Берем первый элемент в очереди
            Vector3 current = toVisit.Dequeue();

            if (current.Equals(target))
            {
                //нашли выход
                found = true;
                break;
            }

            visited.TryAdd(current.GetHashCode(), current);
            List<Vector3> neighbours = GetNeighbours(current, MonsterController.squareSize);

            foreach (Vector3 neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
                {
                    toVisit.Enqueue(neighbour);
                    map.TryAdd(neighbour, current);
                }
            }
        }

        // нашли выход
        if (found)
        {
            List<Vector3> path = new List<Vector3>();
            //обход пути от обратного
            Vector3 key = target;

            do
            {
                path.Add(key);
                key = map[key];
            }
            while (!map[key].Equals(key));

            path.Add(key);

            // развернуть путь
            path.Reverse();
            return path;
        }
        else
        {
            return null;
        }
    }



    // Find neighbour squares
    private static List<Vector3> GetNeighbours(Vector3 square, int squareSize)
    {
        List<Vector3> neighbours = new List<Vector3>();

        //check neighbour positions
        for (int x = -1; x <= 1; ++x)
        {
            for (int z = -1; z <= 1; ++z)
            {
                // diagonals not included
                if (Math.Abs(x) != Math.Abs(z))
                {
                    // Tatget checked position coordinated
                    float targetX = square.x + squareSize * x;
                    float targetZ = square.z + squareSize * z;

                    Vector3 checkedVector = new Vector3(targetX, 0, targetZ);

                    //if raycast failed, we have a spare space
                    if (!Physics.Raycast(square, checkedVector - square, squareSize, 3))
                    {
                        neighbours.Add(checkedVector);
                    }
                }
            }
        }

        return neighbours;
    }

    //private 

    public void MeleeStrikeMonster()
    {
        if (!Player.GetComponent<PlayerStats>().attacked)
        {
            Player.GetComponent<PlayerStats>().attacked = true;
            gameObject.GetComponent<MonsterInfo>().hp -= 1;
            PlayHitAnimation();
            //Player.GetComponent<PlayerStats>().ChangeStamina(1);

            if (Player.GetComponent<PlayerStats>().timeState == TimeState.Original)
            {
                Player.GetComponent<PlayerStats>().ChangeEnergy(1);
            }
            else
            {
                Player.GetComponent<PlayerStats>().TakeDamage(-1);
            }
            //Player.GetComponent<PlayerStats>().ChangeEnergy(1);
            if (gameObject.GetComponent<MonsterInfo>().hp <= 0)
            {
                // death, need to play death animation
                if (gameObject.GetComponent<MonsterController>().timeState == TimeState.Original)
                {
                    Player.GetComponent<PlayerStats>().killCounterOriginal++;
                    if (!Player.GetComponent<PlayerStats>().redKey)
                    {
                        RedKeyRandomDrop(Player.GetComponent<PlayerStats>().killCounterOriginal);
                    }
                }
                else
                {
                    Player.GetComponent<PlayerStats>().killCounterShifted++;
                    if (!Player.GetComponent<PlayerStats>().blueKey)
                    {
                        RedKeyRandomDrop(Player.GetComponent<PlayerStats>().killCounterShifted);
                    }
                }

                DropLoot();
                if (isMoving)
                {
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(destination, false, timeState);
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(startPosition, false, timeState);
                }
                else
                {
                    MazeController.GetComponent<LabirintCreation>().SetOccupation(gameObject.transform.position, false, timeState);
                }
            }
        }
    }

    private void RedKeyRandomDrop(int killCounter)
    {
        if (killCounter <= 5)
        {
            return;
        }
        else
        {
            int chance = (killCounter - 5) * 5;
            int random = new System.Random().Next(0, 100);

            if (random < chance)
            {
                print("You've found the Red Key");
                Player.GetComponent<PlayerStats>().redKey = true;
                GameState.GetComponent<GameState>().redKey.enabled = true;
                return;
            }
        }
    }

    private void BlueKeyRandomDrop(int killCounter)
    {
        if (killCounter <= 5)
        {
            return;
        }
        else
        {
            int chance = (killCounter - 5) * 5;
            int random = new System.Random().Next(0, 100);

            if (random < chance)
            {
                print("You've found the Blue Key");
                Player.GetComponent<PlayerStats>().blueKey = true;
                GameState.GetComponent<GameState>().blueKey.enabled = true;
                return;
            }
        }
    }

    public void DropTimer()
    {
        timer = 0;
    }

    public void PlayHitAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Hit");
    }
    public void PlayAttackAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Attack");
    }


    public void MonsterDie()
    {
        // Remove monster from collection
        if (timeState == TimeState.Original)
        {
            MazeController.GetComponent<LabirintCreation>().Monsters.Remove(gameObject);
        }
        else
        {
            MazeController.GetComponent<LabirintCreation>().MonstersShifted.Remove(gameObject);
        }

        Destroy(gameObject);

        Utils.GetComponent<Utils>().UpdateCursor(gameObject, CursorAction.Default);
    }

    public void DropLoot()
    {
        Vector3 LootPosition = Player.transform.position + Player.transform.forward * 3 + Player.transform.forward * 10;
        Instantiate(LootSack, new Vector3(LootPosition.x, 0, LootPosition.z), Quaternion.identity);
    }

    public bool Hit()
    {
        //print(this.GetType().ToString());
        if (Utils.GetComponent<Utils>().FacingGameObject(gameObject))
        {
            //PlayHitAnimation();
            MeleeStrikeMonster();
            return true;
        }
        else
        {
            return false;
        }
        //hit.transform.gameObject.GetComponent<MonsterController>().PlayHitAnimation();
        //hit.transform.gameObject.GetComponent<MonsterController>().MeleeStrikeMonster();
        //throw new System.NotImplementedException();
    }

    public void OnMouseEnter()
    {
        Utils.GetComponent<Utils>().UpdateCursor(gameObject, CursorAction.Attack);
    }

    public void OnMouseExit()
    {
        Utils.GetComponent<Utils>().UpdateCursor(gameObject);
    }
    public void OnMouseOver()
    {
        if (!Player.GetComponent<PlayerMovement>().moving.Current)
        {
            Utils.GetComponent<Utils>().UpdateCursor(gameObject, CursorAction.Attack);
        }
    }

    public void Shift()
    {
        if (this.timeState == TimeState.Original)
        {
            this.timeState = TimeState.Shifted;
            // Some actions
        }
        else
        {
            this.timeState = TimeState.Original;
            // Some actions
        }
    }
}
